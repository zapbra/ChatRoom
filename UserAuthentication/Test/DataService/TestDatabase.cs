using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using UserAuthentication.Controllers;
using UserAuthentication.DTOs;
using UserAuthentication.Models;
using Xunit;

namespace UserAuthentication.Test.DataService
{
    public class TestDatabase
    {
        private readonly string _testConnectionString = "Host=localhost; Database=userauthtest; Username=authuser; Password=authpassword";
        private AppDbContext _context = null!;
        private ILogger<UsersController> _logger = null;
        private RolesController rolesController = null!;
        private UsersController usersController = null!;
        public TestDatabase()
        {
            CreateContextForTesting();
        }

        public void CreateContextForTesting()
        {
            if (_context == null)
            {
                 var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(_testConnectionString)
                    .Options;

                _context = new AppDbContext(options);

                using var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                });
                _logger = loggerFactory.CreateLogger<UsersController>();

                // initialize controllers
                rolesController = new RolesController(_context);
                usersController = new UsersController(_context, _logger);

                // Delete and recreate the database for a clean slate
                _context.Database.EnsureDeleted();
                _context.Database.EnsureCreated();
            }
        }


        [Fact]
        public void TestDatabase_Add_ExternalProvider()
        {
            // Arrange
            ExternalProvider externalProvider = new ExternalProvider
            {
                ProviderName = "Facebook",
                WSEndpoint = "http://example.com"
            };

            // Act
            _context.Add(externalProvider);
            _context.SaveChanges();

            // Assert
            ExternalProvider firstExternalProvider = _context.ExternalProviders.First();
            firstExternalProvider.ProviderName.Should().Be("Facebook");
            firstExternalProvider.Should().NotBeNull();
        }

        [Fact]
        public void Testing()
        {
            int bla = 5;
            bla.Should().Be(5);
        }


      
        
        [Fact]
        public async void TestDatabase_Add_User()
        {
            // Arrange

            // fetch admin role from database for user
            var actionResult = await rolesController.GetRoleByName("admin");
            Role adminRole = null;
            User user = null;
            UserLogin userLogin = null;
            if (actionResult.Result is OkObjectResult okResult)
            {
                var adminRoleDto = okResult.Value as RoleDto;
                if (adminRoleDto != null)
                {
                    adminRole = new Role()
                    {
                        Id = adminRoleDto.Id,
                        RoleName = adminRoleDto.RoleName
                    };
                }
            }

            if (adminRole != null)
            {
                user = new User()
                {
                    Role = adminRole
                };

                // Created with default values
                EmailValidationStatus emailValidationStatus = new EmailValidationStatus();

                userLogin = new UserLogin()
                {
                    User = user,
                    Username = "Testuser1",
                    PasswordHash = "fakepassword",
                    PasswordSalt = "passwordsalt",
                    Email = "testemail@gmail.com",
                    EmailValidationStatus = emailValidationStatus
                };



                // Act
                _context.Add(userLogin);
                await _context.SaveChangesAsync();

                var userResult = await usersController.GetUserByUsername("Testuser1");
                if (userResult.Result is OkObjectResult okResult2)
                {
                    var userFetch = okResult2.Value as User;

                    userFetch.Should().NotBeNull();
                    if (userFetch != null)
                    {
                        // make sure the inserted user is in the database
                        userFetch.UserLogin.Username.Should().Be(userLogin.Username);
                    }
                    
                }
                

            }

            

          
                
            


            
            


            // Assert



        } 
        /*

        [Fact]
        public void TestDatabase_Add_()
        {

            _context.Add();
            _context.SaveChanges();


        }


        [Fact]
        public void TestDatabase_Add_()
        {

            _context.Add();
            _context.SaveChanges();


        } */
    }
}
