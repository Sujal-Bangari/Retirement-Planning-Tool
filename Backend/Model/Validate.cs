using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace retplann.Model
{
    public class Validate
    {
        
        [Required]
        [EmailAddress]
        public string Email{get; set;}
        [Required]
        public string Password{get; set;}
    }
}
