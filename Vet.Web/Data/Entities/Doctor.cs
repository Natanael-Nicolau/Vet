using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Data.Entities
{
    public class Doctor : IEntity
    {
        public int Id { get; set; }
        public bool IsAproved { get; set; }
        public bool IsDeleted { get; set; }


        public TimeSpan AppointmentDuration { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        

        public int RoomId { get; set; }
        public Room Room { get; set; }

    }
}
