using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Client ToClientEntity(Models.Account.RegisterViewModel model, string userId)
        {
            return new Client
            {
                InsuranceCompanyId = (model.InsuranceCompanyId == 0 ? null : model.InsuranceCompanyId),
                IsAproved = true,
                IsDeleted = false,
                PolicyNumber = model.PolicyNumber,
                UserId = userId
            };
        }

        public Models.Admins.DetailsViewModel ToAdminDetailsViewModel(User user)
        {
            return new Models.Admins.DetailsViewModel
            {
                Id = user.Id,
                SS = user.SS,
                DateOfBirth = user.DateOfBirth,
                FullName = user.FullName,
                Username = user.UserName,
                NIF = user.NIF,
                PictureUrl = user.PictureUrl,
                Role = "Admin"
            };
        }

        public Doctor ToDoctorEntity(Models.Account.CreateViewModel model, string userId)
        {
            return new Doctor
            {
                AppointmentDuration = new TimeSpan(0, model.AppointmentDuration, 0),
                IsAproved = true,
                IsDeleted = true,
                RoomId = model.RoomId,
                UserId = userId
            };
        }

        public Models.Account.IndexViewModel ToIndexViewModel(User user, string role)
        {
            return new Models.Account.IndexViewModel
            {
                EmailConfirmed = user.EmailConfirmed,
                FullName = user.FullName,
                Id = user.Id,
                Role = role,
                Username = user.UserName
            };
        }

        public User ToUserEntity(Models.Account.CreateViewModel model, string path)
        {
            return new User
            {
                DateOfBirth = model.DateOfBirth,
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsDeleted = false,
                NIF = model.NIF,
                PictureUrl = path
            };
        }

        public User ToUserEntity(Models.Account.RegisterViewModel model, string path)
        {
            return new User
            {
                SS = "",
                Email = model.Email,
                UserName = model.Email,
                DateOfBirth = model.DateOfBirth,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsDeleted = false,
                NIF = model.NIF,
                PictureUrl = path
            };
        }

        public Models.Doctors.DetailsViewModel ToDoctorDetailsViewModel(Doctor doctor)
        {
            return new Models.Doctors.DetailsViewModel
            {
                Id = doctor.UserId,
                SpecialityName = doctor.Room.Speciality.Name,
                SS = doctor.User.SS,
                DateOfBirth = doctor.User.DateOfBirth,
                FullName = doctor.User.FullName,
                NIF = doctor.User.NIF,
                PictureUrl = doctor.User.PictureUrl,
                Role = "Doctor",
                RoomName = doctor.Room.Name,
                Username = doctor.User.UserName
            };
        }

        public Models.Clients.DetailsViewModel ToClientDetailsViewModel(Client client)
        {
            return new Models.Clients.DetailsViewModel
            {
                Id = client.UserId,
                SS = client.User.SS,
                DateOfBirth = client.User.DateOfBirth,
                FullName = client.User.FullName,
                NIF = client.User.NIF,
                PictureUrl = client.User.PictureUrl,
                Role = "Client",
                Username = client.User.UserName,
                InsuranceCompanyName = client.InsuranceCompany.Name,
                NumberOfAnimals = client.NumberOfAnimals,
                PolicyNumber = client.PolicyNumber,
                ClientId = client.Id
            };
        }

        public Models.Specialities.DetailsViewModel ToDetailsViewModel(Speciality speciality)
        {
            return new Models.Specialities.DetailsViewModel
            {
                Id = speciality.Id,
                Name = speciality.Name,
                Rooms = speciality.Rooms
            };
        }

        public Models.Specialities.EditViewModel ToEditViewModel(Speciality speciality)
        {
            return new Models.Specialities.EditViewModel
            {
                Id = speciality.Id,
                Name = speciality.Name
            };
        }


        public Speciality ToSpecialityEntity(Models.Specialities.CreateViewModel model)
        {
            return new Speciality
            {
                IsAproved = true,
                IsDeleted = false,
                Name = model.Name
            };
        }

        public Speciality ToSpecialityEntity(Models.Specialities.EditViewModel model)
        {
            return new Speciality
            {
                Id = model.Id,
                IsAproved = true,
                IsDeleted = false,
                Name = model.Name
            };
        }

        public Models.Specialities.CreateRoomViewModel ToCreateRoomViewModel(Speciality speciality)
        {
            return new Models.Specialities.CreateRoomViewModel
            {
                SpecialityId = speciality.Id
            };
        }

        public Models.Specialities.EditRoomViewModel ToEditRoomViewModel(Room room)
        {
            return new Models.Specialities.EditRoomViewModel
            {
                Id = room.Id,
                Name = room.Name,
                SpecialityId = room.SpecialityId
            };
        }

        public Room ToRoomEntity(Models.Specialities.EditRoomViewModel model)
        {
            return new Room
            {
                Id = model.Id,
                Name = model.Name,
                SpecialityId = model.SpecialityId,
                IsAproved = true,
                IsDeleted = false,
            };
        }

        public Models.Species.DetailsViewModel ToDetailsViewModel(Specie specie)
        {
            return new Models.Species.DetailsViewModel
            {
                Id = specie.Id,
                Breeds = specie.Breeds,
                Name = specie.Name
            };
        }

        public Models.Species.EditViewModel ToEditViewModel(Specie specie)
        {
            return new Models.Species.EditViewModel
            {
                Id = specie.Id,
                Name = specie.Name
            };
        }

        public Models.Species.EditBreedViewModel ToEditBreedViewModel(Breed breed)
        {
            return new Models.Species.EditBreedViewModel
            {
                Id = breed.Id,
                SpecieId = breed.SpecieId,
                Name = breed.Name
            };
        }

        public Models.Species.CreateBreedViewModel ToCreateBreedViewModel(Specie specie)
        {
            return new Models.Species.CreateBreedViewModel
            {
                SpecieId = specie.Id
            };
        }

        public Breed ToBreedEntity(Models.Species.EditBreedViewModel model)
        {
            return new Breed
            {
                Id = model.Id,
                SpecieId = model.SpecieId,
                IsAproved = true,
                IsDeleted = false,
                Name = model.Name
            };
        }

        public Specie ToSpecieEntity(Models.Species.CreateViewModel model)
        {
            return new Specie
            {
                IsAproved = true,
                IsDeleted = false,
                Name = model.Name
            };
        }

        public Specie ToSpecieEntity(Models.Species.EditViewModel model)
        {
            return new Specie
            {
                Id = model.Id,
                IsAproved = true,
                IsDeleted = false,
                Name = model.Name
            };
        }

        public Models.InsuranceCompanies.DetailsViewModel ToDetailsViewModel(InsuranceCompany insuranceCompany)
        {
            return new Models.InsuranceCompanies.DetailsViewModel
            {
                Id = insuranceCompany.Id,
                Email = insuranceCompany.Email,
                DiscountPercent = insuranceCompany.DiscountPercent,
                Name = insuranceCompany.Name,
                PhoneNumber = insuranceCompany.PhoneNumber
            };
        }

        public InsuranceCompany ToInsuranceCompanyEntity(Models.InsuranceCompanies.CreateViewModel model)
        {
            return new InsuranceCompany
            {
                IsAproved = true,
                IsDeleted = false,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DiscountPercent = model.DiscountPercent
            };
        }

        public InsuranceCompany ToInsuranceCompanyEntity(Models.InsuranceCompanies.EditViewModel model)
        {
            return new InsuranceCompany
            {
                Id = model.Id,
                IsAproved = true,
                IsDeleted = false,
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DiscountPercent = model.DiscountPercent
            };
        }

        public Models.InsuranceCompanies.EditViewModel ToEditViewModel(InsuranceCompany insuranceCompany)
        {
            return new Models.InsuranceCompanies.EditViewModel
            {
                Id = insuranceCompany.Id,
                Email = insuranceCompany.Email,
                Name = insuranceCompany.Name,
                PhoneNumber = insuranceCompany.PhoneNumber,
                DiscountPercent = insuranceCompany.DiscountPercent
            };
        }

        public Models.Animals.IndexViewModel ToIndexViewModel(Client client)
        {
            return new Models.Animals.IndexViewModel
            {
                ClientId = client.Id,
                FullName = client.User.FullName,
                IndexAnimals = client.Animals.Select(a => new Models.Animals.IndexAnimalViewModel
                {
                    Name = a.Name,
                    SpecieWithBreedName = $"{a.Breed.Specie.Name} {a.Breed.Name}",
                    Age = a.Age,
                    Id = a.Id,
                    PictureUrl = a.PictureUrl
                }).ToList()
            };
        }

        public Models.Animals.DetailsViewModel ToDetailsViewModel(Animal animal)
        {
            return new Models.Animals.DetailsViewModel
            {
                Id = animal.Id,
                SpecieWithBreedName = $"{animal.Breed.Specie.Name} {animal.Breed.Name}",
                Name = animal.Name,
                DateOfBirth = animal.DateOfBirth,
                PictureUrl = animal.PictureUrl,
                Weight = animal.Weight,
                OwnerId = animal.OwnerId
            };
        }

        public Animal ToAnimalEntity(Models.Animals.CreateViewModel model, string path)
        {
            return new Animal
            {
                BreedId = model.BreedId,
                DateOfBirth = model.DateOfBirth,
                IsAproved = true,
                IsDeleted = false,
                OwnerId = model.OwnerId,
                Name = model.Name,
                PictureUrl = path
            };
        }

        public Animal ToAnimalEntity(Models.Animals.EditViewModel model, string path)
        {
            return new Animal
            {
                Id = model.Id,
                BreedId = model.BreedId,
                DateOfBirth = model.DateOfBirth,
                IsAproved = true,
                IsDeleted = false,
                OwnerId = model.OwnerId,
                Name = model.Name,
                PictureUrl = path,
                Weight = model.Weight
            };
        }

        public Models.Animals.EditViewModel ToEditViewModel(Animal animal)
        {
            return new Models.Animals.EditViewModel
            {
                SpecieId = animal.Breed.SpecieId,
                BreedId = animal.BreedId,
                DateOfBirth = animal.DateOfBirth,
                Id = animal.Id,
                Name = animal.Name,
                OwnerId = animal.OwnerId,
                Weight = animal.Weight,
                PictureUrl = animal.PictureUrl
            };
        }

        public Models.Appointments.DetailsViewModel ToDetailsViewModel(Appointment appointment)
        {
            return new Models.Appointments.DetailsViewModel
            {
                Id = appointment.Id,
                AnimalId = appointment.AnimalId,
                Start = appointment.Start,
                End = appointment.End,
                ClientFullName = appointment.Animal.Owner.User.FullName,
                DoctorFullName = appointment.Doctor.User.FullName,
                AnimalName = appointment.Animal.Name,
                SpecialityName = appointment.Doctor.Room.Speciality.Name,
                RoomName = appointment.Doctor.Room.Name
            };
        }
    }
}
