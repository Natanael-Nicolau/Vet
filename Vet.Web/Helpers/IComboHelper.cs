using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Helpers
{
    public interface IComboHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboRolesAsync();

        IEnumerable<SelectListItem> GetComboInsurances();

        IEnumerable<SelectListItem> GetComboSpecialities();
        Task<IEnumerable<SelectListItem>> GetComboRoomsAsync(int specialityId);

        IEnumerable<SelectListItem> GetComboSpecies();
        Task<IEnumerable<SelectListItem>> GetComboBreedsAsync(int specieId);

        Task<IEnumerable<SelectListItem>> GetComboDoctorsAsync(int roomId);

        IEnumerable<SelectListItem> GetComboAvailableAppointment(List<Appointment> reservedAppointments, TimeSpan appointmentTime);
    }
}
