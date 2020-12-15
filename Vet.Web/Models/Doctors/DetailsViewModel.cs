using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Doctors
{
    public class DetailsViewModel : Admins.DetailsViewModel
    {
        [Display(Name ="Room")]
        public string RoomName { get; set; }


        [Display(Name = "Speciality")]
        public string SpecialityName { get; set; }

    }
}
