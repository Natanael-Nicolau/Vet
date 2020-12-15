using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Data.Entities
{
    public class Room : IEntity
    {
        public int Id { get; set; }
        public bool IsAproved { get; set; }
        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public Speciality Speciality { get; set; }
        public int SpecialityId { get; set; }


        public ICollection<Doctor> Doctors { get; set; }
        public int NumberOfDoctors => Doctors == null ? 0 : Doctors.Count(d => !d.IsDeleted);
    }
}
