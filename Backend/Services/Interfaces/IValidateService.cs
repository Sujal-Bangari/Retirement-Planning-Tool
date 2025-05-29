using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using retplann.Model;

namespace retplann.Services.Interfaces
{
    public interface IValidateService
    {
        Task<Profile> Validation(string email, string password);
    }
}
