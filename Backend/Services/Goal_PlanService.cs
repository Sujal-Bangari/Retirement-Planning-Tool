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
    public class Goal_planService : IGoal_planService
    {
        private readonly Repository _repository;
        private readonly ILogger<Goal_planService> _logger;
        public Goal_planService(Repository repository, ILogger<Goal_planService> logger)
        {
            _repository=repository;
            _logger=logger;
        }
        
        public async Task<Goal_plan> StoringGoal(Goal_plan g_p)
        {
            if (g_p == null)
            {
                _logger.LogWarning("Goal Plan is null.");
            }
            _logger.LogInformation("Accessing the Method for storing goal plan");
            return _repository.StoreGoal(g_p);
        }
        public async Task<Goal_plan> GettingUserGoalPlan(int pid)
        {
           if (pid <= 0 || pid==null)
            {
                _logger.LogError("Invalid pid provided: {Pid}", pid);
                return null;
            }
            _logger.LogInformation("Accessing the Method for acquiring goal plan");
            return _repository.GetUserGoalPlan(pid);
        }
        // public async Task Calculation(int pid)
        // {
        //     if (pid <= 0 || pid==null)
        //     {
        //         _logger.LogError("Invalid pid provided: {Pid}", pid);
        //     }
        //     _repository.Calculate(pid);
        // }
    }
}
