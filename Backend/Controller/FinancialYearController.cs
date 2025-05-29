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
    [Route("api/MonthInvestment")]
    public class FinancialYearController : ControllerBase
    {
        private readonly IFinancialYearService _fyService;
        private readonly ILogger<FinancialYearController> _logger;
        public FinancialYearController(IFinancialYearService fyService, ILogger<FinancialYearController> logger){
            _fyService=fyService;
            _logger=logger;
        }

        [HttpPost("Store")]
        public async Task<IActionResult> Store([FromBody]FinancialYear fy)
        {
            if (!ModelState.IsValid) // Check for validation errors
            {
                return StatusCode(StatusCodes.Status400BadRequest, ModelState); // Return error responses
            }
            try
            {
                _logger.LogInformation("Received request to store investment");
                if (fy == null)
                {
                    _logger.LogWarning("Financial Year is null.");
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid request: Financial data is required." });
                }
                if ((fy.GId == null || fy.GId <= 0) || (fy.Year == null || fy.Year <= 0) || (fy.MonthNumber == null || fy.MonthNumber <= 0) || (fy.MonthlyInvestment == null || fy.MonthlyInvestment <= 0))
                {
                    _logger.LogWarning("Invalid data: {FinancialYear}", fy);
                    return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid financial year data. Ensure that there is a value." });
                }
                await _fyService.StoringInvestments(fy);
                _logger.LogInformation("Stored investment successfully");
                return StatusCode(StatusCodes.Status201Created, "");
            }
            catch(Exception ex)
            {
                _logger.LogError("Error: ", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }
    }
}
