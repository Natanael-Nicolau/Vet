using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Appointments
{
    public class DoctorIndexViewModel
    {
        public bool IsApproval { get; set; }


        public ICollection<IndexViewModel> ModelList { get; set; }
    }
}
