using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using retplann.Model;
using retplann.Data;
using Microsoft.Extensions.Logging;
using retplann.Services.Interfaces;

namespace retplann.Services
{
    public class ValidateService : IValidateService
    {
        private readonly Repository _repository;
        private readonly ILogger<ValidateService> _logger;
        public ValidateService(Repository repository, ILogger<ValidateService> logger)
        {
            _repository=repository;
            _logger=logger;
        }
        public async Task<Profile> Validation(string email, string password)
        {
           if(String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password))
            {
                return null;
            } 
            return _repository.ValidateUser(email,password);
        }
       
    }
}
