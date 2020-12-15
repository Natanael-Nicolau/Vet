using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Helpers
{
    public interface IConverterHelper
    {
        Models.Account.IndexViewModel ToIndexViewModel(User user, string role);
        Models.Animals.IndexViewModel ToIndexViewModel(Client client);



        Models.Admins.DetailsViewModel ToAdminDetailsViewModel(User user);
        Models.Doctors.DetailsViewModel ToDoctorDetailsViewModel(Doctor doctor);
        Models.Clients.DetailsViewModel ToClientDetailsViewModel(Client client);
        Models.Specialities.DetailsViewModel ToDetailsViewModel(Speciality speciality);
        Models.Species.DetailsViewModel ToDetailsViewModel(Specie specie);
        Models.InsuranceCompanies.DetailsViewModel ToDetailsViewModel(InsuranceCompany insuranceCompany);
        Models.Animals.DetailsViewModel ToDetailsViewModel(Animal animal);
        Models.Appointments.DetailsViewModel ToDetailsViewModel(Appointment appointment);



        Models.Specialities.EditViewModel ToEditViewModel(Speciality speciality);
        Models.Specialities.EditRoomViewModel ToEditRoomViewModel(Room room);
        Models.Species.EditViewModel ToEditViewModel(Specie specie);
        Models.Species.EditBreedViewModel ToEditBreedViewModel(Breed breed);
        Models.InsuranceCompanies.EditViewModel ToEditViewModel(InsuranceCompany insuranceCompany);
        Models.Animals.EditViewModel ToEditViewModel(Animal animal);



        Models.Specialities.CreateRoomViewModel ToCreateRoomViewModel(Speciality speciality);
        Models.Species.CreateBreedViewModel ToCreateBreedViewModel(Specie specie);





        Room ToRoomEntity(Models.Specialities.EditRoomViewModel model);
        Speciality ToSpecialityEntity(Models.Specialities.CreateViewModel model);
        Speciality ToSpecialityEntity(Models.Specialities.EditViewModel model);

        Breed ToBreedEntity(Models.Species.EditBreedViewModel model);
        Specie ToSpecieEntity(Models.Species.CreateViewModel model);
        Specie ToSpecieEntity(Models.Species.EditViewModel model);
        InsuranceCompany ToInsuranceCompanyEntity(Models.InsuranceCompanies.EditViewModel model);
        InsuranceCompany ToInsuranceCompanyEntity(Models.InsuranceCompanies.CreateViewModel model);

        User ToUserEntity(Models.Account.CreateViewModel model, string path);
        Doctor ToDoctorEntity(Models.Account.CreateViewModel model, string userId);
        User ToUserEntity(Models.Account.RegisterViewModel model, string path);
        Client ToClientEntity(Models.Account.RegisterViewModel model, string userId);
        Animal ToAnimalEntity(Models.Animals.CreateViewModel model, string path);
        Animal ToAnimalEntity(Models.Animals.EditViewModel model, string path);
    }
}
