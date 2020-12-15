using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.InsuranceCompanies
{
    public class EditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        [Display(Name = "Phone Number")]
        [RegularExpression("[0-9]{9}")]
        public string PhoneNumber { get; set; }


        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Discount Percent (%)")]
        [Range(0, 100)]
        public double DiscountPercent { get; set; }
    }
}
