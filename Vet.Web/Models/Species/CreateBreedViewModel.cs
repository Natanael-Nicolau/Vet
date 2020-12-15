using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Species
{
    public class CreateBreedViewModel
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public int SpecieId { get; set; }
    }
}
