using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Data.Entities
{
    public class Specie : IEntity
    {
        public int Id { get; set; }
        public bool IsAproved { get; set; }
        public bool IsDeleted { get; set; }


        public string Name { get; set; }

        public ICollection<Breed> Breeds { get; set; }

        [NotMapped]
        public int NumberOfBreeds => Breeds == null ? 0 : Breeds.Count(b => !b.IsDeleted);
    }
}
