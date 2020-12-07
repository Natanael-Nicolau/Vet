using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Data.Entities
{
    public class Client : IEntity
    {
        public int Id { get; set; }
        public bool IsAproved { get; set; }
        public bool IsDeleted { get; set; }


        public string NIF { get; set; }


        public InsuranceCompany InsuranceCompany { get; set; }
        public int InsuranceId { get; set; }


        
        public string PolicyNumber { get; set; }
        //TODO: EXTRA: Confirmation for Policy number!


        public ICollection<Animal> Animals { get; set; }

        [NotMapped]
        public int NumberOfAnimals => Animals == null ? 0 : Animals.Count;



        public User User { get; set; }
        public string UserId { get; set; }
    }
}
