using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Appointments
{
    public class CreateViewModel
    {
        [Required]
        public int AnimalId { get; set; }

        [Display(Name ="Animal")]
        public string AnimalName { get; set; }


        [Display(Name = "Speciality")]
        [Range(1, int.MaxValue, ErrorMessage = "Speciality is Required.")]
        public int SpecialityId { get; set; }
        public IEnumerable<SelectListItem> Specialities { get; set; }


        [Display(Name = "Room")]
        [Range(1, int.MaxValue, ErrorMessage = "Room is Required.")]
        public int RoomId { get; set; }
        public IEnumerable<SelectListItem> Rooms { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? Day { get; set; }

        [Display(Name = "Appointment Time")]
        [Range(1, int.MaxValue, ErrorMessage = "Appointment Time is Required.")]
        public int AppointmentTime { get; set; }
        public IEnumerable<SelectListItem> AvailableHours { get; set; }



        [Display(Name = "Doctor")]
        [Range(1, int.MaxValue, ErrorMessage = "Doctor is Required.")]
        public int DoctorId { get; set; }
        public IEnumerable<SelectListItem> Doctors { get; set; }



    }
}
