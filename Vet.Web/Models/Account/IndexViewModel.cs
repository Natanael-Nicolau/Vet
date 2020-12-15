using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Account
{
    public class IndexViewModel
    {
        public string Id { get; set; }


        public string Username { get; set; }

        [Display(Name ="Full Name")]
        public string FullName { get; set; }


        [Display(Name ="Email Confirmation?")]
        public bool EmailConfirmed { get; set; }


        public string Role { get; set; }

    }
}
