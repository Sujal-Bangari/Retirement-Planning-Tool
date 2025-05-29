using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using retplann.Model;

namespace retplann.Services.Interfaces
{
    public interface IGoal_planService
    {
        Task<Goal_plan> StoringGoal(Goal_plan g_p);
        Task<Goal_plan> GettingUserGoalPlan(int pid);
        // Task Calculation(int pid);
    }
}
