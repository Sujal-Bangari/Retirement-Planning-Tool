using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace retplann.Model
{
    public class Profile
    {
        public int? PId {get; set;}
        public string FirstName{get; set;}
        public string LastName{get; set;}
        public int? Age{get; set;}
        public string Gender{get; set;} 
        public string Email{get; set;}
    }
}
