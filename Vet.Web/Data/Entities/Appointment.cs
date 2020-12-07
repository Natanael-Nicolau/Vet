using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Data.Entities
{
    public class Appointment : IEntity
    {
        public int Id { get; set; }
        public bool IsAproved { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? Start { get; set; }

        [NotMapped]
        public DateTime? End => (Start.Value + Doctor.AppointmentDuration);



        public Doctor Doctor { get; set; }
        public int DoctorId { get; set; }

        public Animal Animal { get; set; }
        public int AnimalId { get; set; }



    }
}
