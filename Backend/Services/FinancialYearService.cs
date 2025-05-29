using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using retplann.Model;
using retplann.Data;
using retplann.Services.Interfaces;

namespace retplann.Services
{
    public class FinancialYearService : IFinancialYearService
    {
        private readonly Repository _repository;
        private readonly ILogger<FinancialYearService> _logger;
        public FinancialYearService(Repository repository, ILogger<FinancialYearService> logger)
        {
            _repository=repository;
            _logger=logger;
        }
        public async Task StoringInvestments(FinancialYear fy)
        {
            _logger.LogInformation("Accessing the Method for storing month investment");
            if (fy == null)
            {
                _logger.LogWarning("Financial Year is null.");
            }
            _repository.InsertFinancialYear(fy);
        }
    }
}
