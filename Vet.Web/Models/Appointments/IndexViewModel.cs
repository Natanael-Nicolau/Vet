using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Vet.Web.Data.Entities;

namespace Vet.Web.Models.Appointments
{
    public class IndexViewModel
    {
        public int Id { get; set; }

        public bool IsAproved { get; set; }


        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime? Start { get; set; }


        [DisplayFormat(DataFormatString = "{0:g}")]
        public DateTime? End { get; set; }

        [Display(Name ="Animal")]
        public string AnimalName { get; set; }

        [Display(Name = "Doctor")]
        public string DoctorName { get; set; }
    }
}
