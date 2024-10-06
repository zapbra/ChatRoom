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
using UserAuthentication.Utilities;

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
                .Where(u => u.UserLogin != null && u.UserLogin.Username  == username)
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

        // Used to get the current hashing algorithm id, mainly for user inserts
        // The purpose of this function is if a specific hashing algorithm is used in multiple places
        // this will hopefully be maintainable (over-engineered much)
        private int GetHashingAlgorithmId()
        {
            return 1;
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
                    HashAlgorithmId = this.GetHashingAlgorithmId(),
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

                var userCookieDict = new Dictionary<string, string>
                {
                    { "user_id", newUser.Id.ToString()},
                    { "username", newUserLogin.Username},
                    { "email", newUserLogin.Email},
                    { "authenticated", newUserState.Authenticated.ToString() },
                    { "auth_expiry_time", newUserState.ExpiryTime.ToString()},
                    { "user_state_id", newUserState.StateUUID},
                    { "is_email_validated", newUserLogin.EmailValidationStatus.IsValidated.ToString() ?? "false" },
                    { "email_status_description", newUserLogin.EmailValidationStatus.StatusDescription ?? "Pending"},
                };

                CookieUtility.AddCookies(Response, userCookieDict);

                await transcation.CommitAsync();



                // returns the state of the insert
                return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, newUser);
            }
            catch (Exception ex)
            {
                await transcation.RollbackAsync();

                _logger.LogError(ex, "An eror occured while creaing a new user");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while creating the user: " + ex.Message);
            }


        }

        // POST: api/Users/
        // UserLoginDto is passed either the email or username based on which the user
        // decides to enter and queries for user that contains that username or email
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserLoginDto userDto)
        {

            try
            {
                // find user from database comparing the email and username based on the user entered field
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => (u.UserLogin != null && u.UserLogin.Username == userDto.Username)
                    || (u.UserLogin != null && u.UserLogin.Email == userDto.Email));

                // make sure user was actually found
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // hash client pass with salt to compare to fetched user
                var clientHashedPass = HashAlgorithmUtility.HashPassword($"{userDto.Password}{user?.UserLogin?.PasswordSalt ?? ""}");

                // wrong password
                if (clientHashedPass != user?.UserLogin?.PasswordHash)
                {
                    return Unauthorized("Invalid credentials");

                }

                // At this point, the user has entered the correct password and it can 
                // cookies can be updated to reflect the login state
                var userCookieDict = new Dictionary<string, string>
                {
                    { "user_id", user.Id.ToString()},
                    { "username", user.UserLogin.Username},
                    { "email", user.UserLogin.Email},
                    { "authenticated", user.UserState.Authenticated.ToString() },
                    { "auth_expiry_time", user.UserState.ExpiryTime.ToString()},
                    { "user_state_id", user.UserState.StateUUID},
                    { "is_email_validated", user.UserLogin.EmailValidationStatus.IsValidated.ToString() ?? "false" },
                    { "email_status_description", user.UserLogin.EmailValidationStatus.StatusDescription ?? "Pending"},
                };

                CookieUtility.AddCookies(Response, userCookieDict);

                return Ok(user);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while logging in");
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
        }


        // GET: api/Users/Userlogin/5
        [HttpGet("Userlogin/{id}")]
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
