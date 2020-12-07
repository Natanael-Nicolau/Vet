using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Data.Entities
{
    public class Animal : IEntity
    {
        public int Id { get; set; }
        public bool IsAproved { get; set; }
        public bool IsDeleted { get; set; }


        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public double? Weight { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                if (DateOfBirth == null)
                {
                    return 0;
                }
                var myAge = DateTime.Now.Year - DateOfBirth.Value.Year;
                if (DateOfBirth > DateTime.Now.AddYears(-myAge))
                {
                    myAge--;
                }
                return myAge;
            }
        }
        public Breed Breed { get; set; }
        public int BreedId { get; set; }

        public Client Owner { get; set; }
        public int OwnerId { get; set; }
    }
}
