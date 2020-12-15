using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Models.Species
{
    public class DetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Breed> Breeds { get; set; }
    }

}
