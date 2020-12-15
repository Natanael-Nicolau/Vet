using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Clients
{
    public class DetailsViewModel : Admins.DetailsViewModel
    {
        [Display(Name ="Insurance Company")]
        public string InsuranceCompanyName { get; set; }

        public int ClientId { get; set; }

        [Display(Name = "Policy Number")]
        public string PolicyNumber { get; set; }

        [Display(Name ="# Animals")]
        public int NumberOfAnimals { get; set; }
    }
}
