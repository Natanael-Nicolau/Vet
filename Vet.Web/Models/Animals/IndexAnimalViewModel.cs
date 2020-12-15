using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Animals
{
    public class IndexAnimalViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name ="Full Breed")]
        public string SpecieWithBreedName { get; set; }

        public int Age { get; set; }

        [Display(Name ="Photo")]
        public string PictureUrl { get; set; }
    }
}
