using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Appointments
{
    public class DetailsViewModel
    {
        public int Id { get; set; }

        public int AnimalId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string AnimalName { get; set; }
        public string ClientFullName { get; set; }
        public string DoctorFullName { get; set; }

        public string RoomName { get; set; }
        public string SpecialityName { get; set; }

    }
}
