using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;
using Vet.Web.Data.Repositories;

namespace Vet.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserRepository _userHelper;
        private readonly Random _random;

        public SeedDb(DataContext context, IUserRepository userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            //adding Roles
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Client");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Doctor");


            //Adding Seed User
            var user = await _userHelper.GetUserByEmailAsync("admin@yopmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Natanael",
                    LastName = "Nicolau",
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    PhoneNumber = "915996181"
                };

                var result = await _userHelper.AddUserAsync(user, "P@ssw0rd");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in Seeder");
                }

                //add user to role
                var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
                if (!isInRole)
                {
                    await _userHelper.AddUserToRoleAsync(user, "Admin");
                }

                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }
        }
    }
}
