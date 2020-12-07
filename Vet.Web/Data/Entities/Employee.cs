using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Data.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }
        public bool IsAproved { get; set; }
        public bool IsDeleted { get; set; }

        public string NIF { get; set; }
        public string CC { get; set; }
        public string SS { get; set; }


        public string PictureUrl { get; set; }

        public DateTime? DateOfBirth { get; set; }


        [NotMapped]
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


        public User User { get; set; }
        public string UserId { get; set; }
    }
}
