using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Models.Specialities
{
    public class DetailsViewModel
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
