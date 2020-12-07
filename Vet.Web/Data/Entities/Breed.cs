using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Data.Entities
{
    public class Breed : IEntity
    {
        public int Id { get; set; }
        public bool IsAproved { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public int SpecieId { get; set; }

        public Specie Specie { get; set; }
    }
}
