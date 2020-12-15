using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Account
{
    public class RegisterViewModel
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
        [RegularExpression("[0-9]{9}", ErrorMessage = "Invalid NIF")]
        public string NIF { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date", Prompt="Birth Date...")]
        public DateTime? DateOfBirth { get; set; }



        [Display(Prompt = "Policy Number...")]
        public string PolicyNumber { get; set; }

        [Display(Name = "Insurance Company")]
        //[Range(1, int.MaxValue)]
        public int? InsuranceCompanyId { get; set; }

        public IEnumerable<SelectListItem> InsuranceCompanies { get; set; }

    }
}
