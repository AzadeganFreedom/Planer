using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Planer.Class;
using Planer.Hashing;
using Planer.Models;
using Planer.Repositories;

namespace Planer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : ControllerBase
    {
        private readonly PlanerDbContext _context;
        private readonly UserRepository userRepo;
        private readonly ListRepository listRepo;

        public UserController(PlanerDbContext context, UserRepository repo1, ListRepository repo2)
        {
            _context = context;
            this.userRepo = repo1;
            this.listRepo = repo2;
        }

        //##########################################################################################################

        // Create: 
        [HttpPost("CreateUser")] // Create User
        public async Task<ActionResult> CreateUserAsync(User user)
        {
            if (user == null)
            {
                return BadRequest("User is null.");
            }

            // Check if UserName already exists
            var userCheck = await userRepo.GetUserByUserNameAsync(user.UserName);
            if (userCheck == null) // If it doesn't exist, create user
            {
                // Hash the password using BCrypt
                var bcryptHandler = new BCryptHandler();
                user.Password = bcryptHandler.BCryptHashing(user.Password);

                // Creates new user
                var newUserCreated = await userRepo.CreateUserAsync(user);

                // Creates new list for the user
                var newListCreated = await listRepo.CreateListAsync(newUserCreated.Id);
            }
            else // If it does exist, BadRequest
            {
                return BadRequest("User already exists.");
            }

            // Return the created user
            return Ok(user);
        }


        //##########################################################################################################

        //Read: 
        [HttpGet("GetAllUsers")] // Get all users
        public async Task<ActionResult> GetAllUsersAsync()
        {
            var allUsers = await userRepo.GetAllUsersAsync();
            return Ok(allUsers);
        }


        [HttpGet("{id:int}")] // Get user by id
        public async Task<ActionResult> GetUserByIdAsync(int id)
        {
            var selectedUser = await userRepo.GetUserByIdAsync(id);

            if (selectedUser == null)
            {
                return NotFound();
            }
            return Ok(selectedUser);
        }

        [HttpGet("GetUserByUserName")] // Get user by userName
        public async Task<ActionResult> GetUserByUserNameAsync(string userName, string password)
        {
            // Fetch user
            var selectedUser = await userRepo.GetUserByUserNameAsync(userName);

            // If user doesn't exist, NotFound
            if (selectedUser == null)
            {
                return NotFound("User not found.");
            }

            // Verify Hashed password
            var bcryptHandler = new BCryptHandler();
            bool isPasswordValid = bcryptHandler.BCryptVerifyHashing(password, selectedUser.Password);

            if (!isPasswordValid) // If password is not valid, Unauthorized
            {
                return Unauthorized("Invalid password");
            }

            // Hide password
            selectedUser.Password = null;

            return Ok(selectedUser);
        }

        //##########################################################################################################

        // Update:
        [HttpPatch("UpdateUserByUserName")] // Update user by UserName
        public async Task<ActionResult> UpdateUserByUserNameAsync(string userName, [FromBody] UpdateUserDto updatedUserData)
        {
            if (updatedUserData == null)
            {
                return BadRequest("User is null.");
            }

            // Check if UserName already exists
            var userCheck = await userRepo.GetUserByUserNameAsync(userName);
            if (userCheck == null) // If it doesn't exist, NotFound
            {
                return NotFound("User not found.");
            }

            // Hash the password using BCrypt if password is not null
            if (!string.IsNullOrEmpty(updatedUserData.Password))
            {
                // Hash the password using BCrypt
                var bcryptHandler = new BCryptHandler();
                updatedUserData.Password = bcryptHandler.BCryptHashing(updatedUserData.Password);
            }
            else // If password is null, keep the old password
            {
                updatedUserData.Password = userCheck.Password;
            }

            // Update user
            var updatedUser = await userRepo.UpdateUserByUserNameAsync(userName, updatedUserData);

            // Return the updated user
            return Ok(updatedUser);
        }

        //##########################################################################################################

        // Post:

        [HttpPost("Login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginDto login)
        {
            if (login == null)
                return BadRequest("Invalid login request.");

            var selectedUser = await userRepo.GetUserByUserNameAsync(login.UserName);

            if (selectedUser == null)
                return NotFound("User not found.");

            var bcryptHandler = new BCryptHandler();
            bool isPasswordValid = bcryptHandler.BCryptVerifyHashing(login.Password, selectedUser.Password);

            if (!isPasswordValid)
                return Unauthorized("Invalid password.");

            selectedUser.Password = null; // Hide password
            return Ok(selectedUser);
        }


        //##########################################################################################################

        // Delete:
    }

}
