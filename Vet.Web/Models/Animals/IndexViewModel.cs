using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Animals
{
    public class IndexViewModel
    {
        public int ClientId { get; set; }

        public string FullName { get; set; }

        public ICollection<IndexAnimalViewModel> IndexAnimals { get; set; }
    }
}
