using System;
using Microsoft.EntityFrameworkCore;
using Planer.Class;
using Planer.Models;

namespace Planer.Repositories
{
    public class UserRepository
    {

        private readonly PlanerDbContext _context;
        public UserRepository(PlanerDbContext context)
        {
            _context = context;
        }

        //##########################################################################################################

        // Create: 

        // Create User: 
        public async Task<User>? CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        //##########################################################################################################

        // Read: 

        // Get all Users:
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var listOfUsers = await _context.Users.ToListAsync();
            return listOfUsers;
        }

        // Get User by Id:
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        //Get User by UserName:
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.UserName == userName);
        }

        //##########################################################################################################

        // Update:

        //Update User by UserName
        public async Task<User>? UpdateUserByUserNameAsync(string userName, UpdateUserDto updatedUserData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == userName);

            user.UserName = updatedUserData.UserName;
            user.Email = updatedUserData.Email;
            user.Password = updatedUserData.Password;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }


        //##########################################################################################################

        // Delete:
    }
}
