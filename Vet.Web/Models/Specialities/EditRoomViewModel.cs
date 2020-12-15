using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Specialities
{
    public class EditRoomViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int SpecialityId { get; set; }


        [Required]
        public string Name { get; set; }
    }
}
