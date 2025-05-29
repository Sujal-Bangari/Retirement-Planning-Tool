using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using retplann.Model;
using retplann.Data;
using Microsoft.Extensions.Logging;
using retplann.Services.Interfaces;

namespace retplann.Services
{
    public class ProgressService : IProgressService
    {
        private readonly Repository _repository;
        private readonly ILogger<ProgressService> _logger;
        public ProgressService(Repository repository, ILogger<ProgressService> logger)
        {
            _repository=repository;
            _logger=logger;
        }
        public async Task<Progress> gettingSavings(int gid)
        {
            if (gid <= 0 || gid==null)
            {
                _logger.LogError("Invalid gid provided: {Gid}", gid);
                return null;
            }
            _logger.LogInformation("Accessing the Method for acquiring savings");
            return _repository.getSavings(gid);
        }
    }
}
