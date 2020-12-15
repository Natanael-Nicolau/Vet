using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Admins
{
    public class DetailsViewModel
    {
        public string Id { get; set; }

        public string NIF { get; set; }
        public string SS { get; set; }
        public string PictureUrl { get; set; }

        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DateOfBirth { get; set; }
        
        public string Username { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }
    }
}
