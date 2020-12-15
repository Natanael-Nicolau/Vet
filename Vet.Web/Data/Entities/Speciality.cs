using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Data.Entities
{
    public class Speciality : IEntity
    {
        public int Id { get; set; }
        public bool IsAproved { get; set; }
        public bool IsDeleted { get; set; }


        public string Name { get; set; }

        public ICollection<Room> Rooms { get; set; }

        [NotMapped]
        public int NumberOfRooms => Rooms == null ? 0 : Rooms.Count(r => !r.IsDeleted);
    }
}
