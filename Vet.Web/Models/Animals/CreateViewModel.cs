using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Animals
{
    public class CreateViewModel
    {
        [Required]
        public int OwnerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public IFormFile Photo { get; set; }



        [Display(Name = "Species")]
        [Range(1, int.MaxValue)]
        public int SpecieId { get; set; }
        public IEnumerable<SelectListItem> Species { get; set; }

        [Display(Name = "Breed")]
        [Range(1, int.MaxValue)]
        public int BreedId { get; set; }
        public IEnumerable<SelectListItem> Breeds { get; set; }

    }
}
