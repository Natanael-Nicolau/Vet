using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Specialities
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "# Rooms")]
        public int NumberOfRooms { get; set; }
    }
}
