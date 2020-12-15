using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Species
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name ="# Breeds")]
        public int NumberOfBreeds { get; set; }
    }
}
