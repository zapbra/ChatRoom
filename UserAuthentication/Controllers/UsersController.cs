using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthentication.DTOs;
using UserAuthentication.Models;
using UserAuthentication.Security;

namespace UserAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersController> _logger;

        public UsersController(AppDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // User Methods

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserLogin)
                .Include(u => u.UserLoginDataExternal)
                .ToListAsync();
        }

        

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }


        // GET: api/Users/Username/johndoe
        [HttpGet("Username/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            var user = await _context.Users
                .Where(u => u.UserLogin.Username == username)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }


            return user;
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }




        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        // UserLogin methods

        // GET: api/Users/Userlogins
        [HttpGet("Userlogins")]
        public async Task<ActionResult<IEnumerable<UserLogin>>> GetUserLogins()
        {
            return await _context.UserLogins.ToListAsync();
        }

        // Post: api/Users/Userlogins
        [HttpPost("Userlogins")]
        public async Task<ActionResult<User>> CreateUser(UserLoginDto userDto)
        {

            using var transcation = await _context.Database.BeginTransactionAsync();
            try
            {
                // create new user, only requires role id
                User newUser = new User
                {
                    RoleId = userDto.User.RoleId,

                };

                // add user to db
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                string salt = HashAlgorithmUtility.GenerateSalt();
                string hashedPassword = HashAlgorithmUtility.HashPassword($"{userDto.Password}{salt}");


                // create new user login
                UserLogin newUserLogin = new UserLogin
                {
                    UserId = newUser.Id,
                    Username = userDto.Username,
                    PasswordHash = hashedPassword,
                    PasswordSalt = salt,
                    Email = userDto.Email,
                };


                // add user login to db
                _context.UserLogins.Add(newUserLogin);
                await _context.SaveChangesAsync();


                // create new user state
                UserState newUserState = new UserState
                {
                    ExpiryTime = DateTime.UtcNow.AddMonths(2), // expires in 2 months, might be changed or check with user if they want to stay logged in
                    UserId = newUser.Id
                };


                // add user state to database
                _context.UserStates.Add(newUserState);
                await _context.SaveChangesAsync();

                await transcation.CommitAsync();

                // returns the state of the insert
                return CreatedAtAction(nameof(GetUserLogin), new { id = newUserLogin.Id }, newUserLogin);
            } catch (Exception ex)
            {
                await transcation.RollbackAsync();

                _logger.LogError(ex, "An eror occured while creaing a new user");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while creating the user: " + ex.Message);
            }
            

        }

        // GET: api/Users/Userlogin
        [HttpGet("Userlogin")]


        // GET: api/Users/Userlogin/5
        [HttpGet("/Userlogin/{id}")]
        public async Task<ActionResult<UserLogin>> GetUserLogin(long id)
        {
            var userLogin = await _context.UserLogins.FindAsync(id);

            if (userLogin == null)
            {
                return NotFound();
            }


            return userLogin;
        }


    }
}
