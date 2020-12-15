using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.InsuranceCompanies
{
    public class DetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }


        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


        public string Email { get; set; }



        [Display(Name = "Discount Percent (%)")]
        public double DiscountPercent { get; set; }
    }
}
