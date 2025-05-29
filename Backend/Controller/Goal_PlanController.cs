using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using retplann.Model;
using retplann.Services;
using retplann.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace retplann.Controller
{
    [ApiController]
    [Route("api/GoalPlan")]
    public class Goal_PlanController : ControllerBase
    {
        private readonly IGoal_planService _goalplanService;
        private readonly ILogger<Goal_PlanController> _logger;
        public Goal_PlanController(IGoal_planService goalplanService, ILogger<Goal_PlanController> logger){
            _goalplanService=goalplanService;
            _logger=logger;
        }

        // [HttpPost("Store")]
        // public async Task<IActionResult> Store([FromBody]Goal_plan g_p)
        // {
        //     if (!ModelState.IsValid) // Check for validation errors
        //     {
        //         return StatusCode(StatusCodes.Status400BadRequest, ModelState); // Return error responses
        //     }
        //     try
        //     {
        //         _logger.LogInformation("Received request to store goal plan");
        //         if (g_p == null)
        //         {
        //             _logger.LogWarning("Goal plan is null.");
        //             return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid request: Goal plan data is required." });
        //         }
        //         if ((g_p.PId == null || g_p.PId <= 0) || (g_p.CurrentAge == null || g_p.CurrentAge <= 0) || (g_p.RetirementAge == null || g_p.RetirementAge <= 0) || (g_p.CurrentSavings == null || g_p.CurrentSavings <= 0) || (g_p.TargetSavings == null || g_p.TargetSavings <= 0))
        //         {
        //             _logger.LogWarning("Invalid goal plan data: {GoalPlan}", g_p);
        //             return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid goal plan data. Ensure that there is a value." });
        //         }
        //         var result=await _goalplanService.StoringGoal(g_p);
        //         if(result==null)
        //         {
        //             _logger.LogWarning("Not Added properly");
        //             return StatusCode(StatusCodes.Status404NotFound, result);
        //         }
        //         return StatusCode(StatusCodes.Status201Created, result);
        //         _logger.LogInformation("Stored goal plan successfully");
        //         return StatusCode(StatusCodes.Status201Created, "Goal Plan stored successfully");
        //     }
        //     catch(Exception ex)
        //     {
        //         _logger.LogError("Error: ", ex.Message);
        //         return StatusCode(StatusCodes.Status500InternalServerError, "Error");
        //     }
            
        // }

        // [HttpGet("calculate/{pid}")]
        // public async Task<IActionResult> Calculation(int pid)
        // {
        //     if (pid <= 0)
        //     {
        //         _logger.LogWarning("Invalid pid provided: {Pid}", pid);
        //         return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid pid value." });
        //     }
        //     try
        //     {
        //         _logger.LogInformation("Receieved request to acquire goal plan");
        //         await _goalplanService.Calculation(pid);
        //         var result=await _goalplanService.GettingUserGoalPlan(pid);
        //         if(result==null)
        //         {
        //             _logger.LogWarning("No goal plan found for User: {Pid}", pid);
        //             return StatusCode(StatusCodes.Status404NotFound, result);
        //         }
        //         return StatusCode(StatusCodes.Status200OK, result);
        //     }
        //     catch(Exception ex)
        //     {
        //         _logger.LogError("Error: ", ex.Message);
        //         return StatusCode(StatusCodes.Status404NotFound);
        //     }
            
        // }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody] Goal_plan g_p)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState); 
            }

            try
            {
                _logger.LogInformation("Received request to store goal plan");

                if (g_p == null)
                {
                    _logger.LogWarning("Goal plan is null.");
                     return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid request: Goal plan data is required." });
                }

                if ((g_p.PId == null || g_p.PId <= 0) || 
                    (g_p.CurrentAge == null || g_p.CurrentAge <= 0) || 
                    (g_p.RetirementAge == null || g_p.RetirementAge <= 0) || 
                    (g_p.CurrentSavings == null || g_p.CurrentSavings < 0) || 
                    (g_p.TargetSavings == null || g_p.TargetSavings <= 0))
                {
                    _logger.LogWarning("Invalid goal plan data: {GoalPlan}", g_p);
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid goal plan data. Ensure that there is a value." });
                }
                var storedGoalPlan = await _goalplanService.StoringGoal(g_p);

                if (storedGoalPlan == null)
                {
                    _logger.LogWarning("Goal plan was not added properly.");
                     return StatusCode(StatusCodes.Status404NotFound, storedGoalPlan);
                }

                _logger.LogInformation("Stored goal plan successfully");
                return StatusCode(StatusCodes.Status201Created, storedGoalPlan);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error storing goal plan: {Message}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Internal server error while storing goal plan." });
            }
        }


        [HttpGet("Retrieve/{pid}")]
        public async Task<IActionResult> Retrieval(int pid)
        {
            if (pid <= 0)
            {
                _logger.LogWarning("Invalid pid provided: {Pid}", pid);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid pid value." });
            }
            try
            {
                _logger.LogInformation("Receieved request to acquire goal plan");
                var result=await _goalplanService.GettingUserGoalPlan(pid);
                if(result==null)
                {
                    _logger.LogWarning("No goal plan found for User: {Pid}", pid);
                    return StatusCode(StatusCodes.Status404NotFound, result);
                }
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch(Exception ex)
            {
                _logger.LogError("Error: ", ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }

        }
    }
}
