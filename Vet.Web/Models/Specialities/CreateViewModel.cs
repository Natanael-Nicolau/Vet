﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vet.Web.Models.Specialities
{
    public class CreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
