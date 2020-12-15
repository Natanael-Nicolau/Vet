using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Account
{
    public class CreateViewModel
    {

        [Required]
        [Display(Name = "First Name", Prompt = "First Name...")]
        [RegularExpression("[a-zA-Z\\u00C0-\\u00D6\\u00D8-\\u00F6\\u00F8-\\u024F]{2,50}", ErrorMessage = "Invalid Characters used")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name", Prompt = "Last Name...")]
        [RegularExpression("[a-zA-Z\\u00C0-\\u00D6\\u00D8-\\u00F6\\u00F8-\\u024F]{2,50}", ErrorMessage = "Invalid Characters used")]
        public string LastName { get; set; }

        [Required]
        [Display(Prompt = "Email...")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Prompt = "NIF...")]
        [RegularExpression("[0-9]{9}", ErrorMessage ="Invalid NIF")]
        public string NIF { get; set; }

        [Required]
        [Display(Prompt = "SS...")]
        [RegularExpression("[0-9]{12}", ErrorMessage = "Invalid SS")]
        public string SS { get; set; }


        public string Username => Email;


        public IFormFile Photo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }



        //DOCTOR ONLY STUFF
        [Display(Name = "Appointment Duration (min)")]
        //[Range(1, 719)] //720 minutos = 12 horas
        public int AppointmentDuration { get; set; }


        [Display(Name = "Speciality")]
        //[Range(1, int.MaxValue)]
        public int SpecialityId { get; set; }

        public IEnumerable<SelectListItem> Specialities { get; set; }

        [Display(Name = "Room")]
        //[Range(1, int.MaxValue)]
        public int RoomId { get; set; }


        public IEnumerable<SelectListItem> Rooms { get; set; }
    }
}
