using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Account
{
    public class ChangePasswordViewModel
    {
        
        [Required]
        [Display(Name ="Old Password")]
        public string OldPassword { get; set; }


       
        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }


        [Required]
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }
    }
}
