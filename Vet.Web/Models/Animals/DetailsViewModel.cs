using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Animals
{
    public class DetailsViewModel
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public double? Weight { get; set; }

        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DateOfBirth { get; set; }


        [Display(Name = "Full Breed")]
        public string SpecieWithBreedName { get; set; }


    }
}
