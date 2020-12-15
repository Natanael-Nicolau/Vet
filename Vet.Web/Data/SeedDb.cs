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
        private readonly IUserRepository _userRepository;

        public SeedDb(DataContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            //adding Roles
            await _userRepository.CheckRoleAsync("Admin");
            await _userRepository.CheckRoleAsync("Client");
            await _userRepository.CheckRoleAsync("Doctor");


            //Adding Seed User
            var user = await _userRepository.GetUserByEmailAsync("admin@yopmail.com");
            if (user == null)
            {
                #region Admin
                user = new User
                {
                    FirstName = "Natanael",
                    LastName = "Nicolau",
                    Email = "admin@yopmail.com",
                    UserName = "admin@yopmail.com",
                    PhoneNumber = "915996181",
                    SS = "123123123123",
                    NIF = "123123123",
                    DateOfBirth = new DateTime(1995, 04, 25),
                    IsDeleted = false,
                    PictureUrl = $"~/images/Users/Default_User_Image.png"
                };

                var result = await _userRepository.AddUserAsync(user, "123123");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in Seeder");
                }

                //add user to role
                var isInRole = await _userRepository.IsUserInRoleAsync(user, "Admin");
                if (!isInRole)
                {
                    await _userRepository.AddUserToRoleAsync(user, "Admin");
                }

                var token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
                await _userRepository.ConfirmEmailAsync(user, token);
                #endregion

                #region Doctor
                user = new User
                {
                    FirstName = "Doctor",
                    LastName = "Pepper",
                    Email = "doctor@yopmail.com",
                    UserName = "doctor@yopmail.com",
                    PhoneNumber = "915996181",
                    SS = "123123123123",
                    NIF = "123123123",
                    DateOfBirth = new DateTime(1995, 04, 25),
                    IsDeleted = false,
                    PictureUrl = $"~/images/Users/Default_User_Image.png"
                };

                result = await _userRepository.AddUserAsync(user, "123123");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in Seeder");
                }

                //add user to role
                isInRole = await _userRepository.IsUserInRoleAsync(user, "Doctor");
                if (!isInRole)
                {
                    await _userRepository.AddUserToRoleAsync(user, "Doctor");
                }

                token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
                await _userRepository.ConfirmEmailAsync(user, token);
                #endregion

                #region Client
                user = new User
                {
                    FirstName = "Peter",
                    LastName = "Pan",
                    Email = "client@yopmail.com",
                    UserName = "client@yopmail.com",
                    PhoneNumber = "915996181",
                    SS = "123123123123",
                    NIF = "123123123",
                    DateOfBirth = new DateTime(1995, 04, 25),
                    IsDeleted = false,
                    PictureUrl = $"~/images/Users/Default_User_Image.png"
                };

                result = await _userRepository.AddUserAsync(user, "123123");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in Seeder");
                }

                //add user to role
                isInRole = await _userRepository.IsUserInRoleAsync(user, "Client");
                if (!isInRole)
                {
                    await _userRepository.AddUserToRoleAsync(user, "Client");
                }

                token = await _userRepository.GenerateEmailConfirmationTokenAsync(user);
                await _userRepository.ConfirmEmailAsync(user, token);
                #endregion
            }

            if (!_context.InsuranceCompanies.Any())
            {
                _context.InsuranceCompanies.Add(new InsuranceCompany { IsAproved = true, IsDeleted = false, Name = "Allianze", Email = "allianze@support.com", PhoneNumber = "123123123", DiscountPercent = 20 });
                await _context.SaveChangesAsync();
            }
            if (!_context.Species.Any())
            {
                _context.Species.Add(new Specie { IsAproved = true, IsDeleted = false, Name = "Dog" });
                _context.Species.Add(new Specie { IsAproved = true, IsDeleted = false, Name = "Cat" });
                _context.Species.Add(new Specie { IsAproved = true, IsDeleted = false, Name = "Bird" });
                await _context.SaveChangesAsync();
            }
            if (!_context.Breeds.Any())
            {
                _context.Breeds.Add(new Breed { IsAproved = true, IsDeleted = false, Name = "German Shepperd", SpecieId = 1, });
                _context.Breeds.Add(new Breed { IsAproved = true, IsDeleted = false, Name = "Terrier", SpecieId = 1, });
                _context.Breeds.Add(new Breed { IsAproved = true, IsDeleted = false, Name = "MaineCoon", SpecieId = 2, });
                await _context.SaveChangesAsync();
            }
            if (!_context.Specialities.Any())
            {
                _context.Specialities.Add(new Speciality { IsDeleted = false, IsAproved = true, Name = "Animal welfare" });
                _context.Specialities.Add(new Speciality { IsDeleted = false, IsAproved = true, Name = "Dermatology" });
                _context.Specialities.Add(new Speciality { IsDeleted = false, IsAproved = true, Name = "Dentistry" });
                await _context.SaveChangesAsync();
            }
            if (!_context.Rooms.Any())
            {
                _context.Rooms.Add(new Room { IsDeleted = false, IsAproved = true, Name = "1", SpecialityId = 1 });
                _context.Rooms.Add(new Room { IsDeleted = false, IsAproved = true, Name = "2", SpecialityId = 1 });
                _context.Rooms.Add(new Room { IsDeleted = false, IsAproved = true, Name = "Home Emergency", SpecialityId = 2 });
                await _context.SaveChangesAsync();
            }
            if (!_context.Doctors.Any())
            {
                user = await _userRepository.GetUserByEmailAsync("doctor@yopmail.com");
                if (user != null)
                {
                    _context.Doctors.Add(new Doctor { IsAproved = true, IsDeleted = false, RoomId = 1, UserId = user.Id, AppointmentDuration = new TimeSpan(0, 30, 0) });
                    await _context.SaveChangesAsync();
                }
            }
            if (!_context.Clients.Any())
            {
                user = await _userRepository.GetUserByEmailAsync("client@yopmail.com");
                if (user != null)
                {
                    _context.Clients.Add(new Client { IsAproved = true, IsDeleted = false, InsuranceCompanyId = 1, UserId = user.Id, PolicyNumber="123ijh12j3asd" });
                    await _context.SaveChangesAsync();
                }
            }
            if (!_context.Animals.Any())
            {
                _context.Animals.Add(new Animal { IsAproved = true, IsDeleted = false, BreedId = 1, Name = "Jonhy boy", DateOfBirth =new DateTime(2010, 1, 1), OwnerId = 1, PictureUrl = $"~/images/Animals/seedDogo.jpg", Weight = 80 });
                await _context.SaveChangesAsync();
            }



        }
    }
}
