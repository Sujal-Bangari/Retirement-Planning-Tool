using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using retplann.Model;
using retplann.Services;
using retplann.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace retplann.Controller
{
    public class ProgressController : ControllerBase
    {
        private readonly IProgressService _progressService;
        private readonly ILogger<ProgressController> _logger;
        public ProgressController(IProgressService progressService, ILogger<ProgressController> logger){
            _progressService=progressService;
            _logger=logger;
        }
        [HttpGet("Savings/{gid}")]
        public async Task<IActionResult> Retrieval([FromRoute] int gid)
        {
            _logger.LogInformation("Received gid: {Gid}", gid);
            if (gid <= 0)
            {
                _logger.LogWarning("Invalid gid provided: {Gid}", gid);
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Invalid gid value." });
            }
            try
            {
                _logger.LogInformation("Receieved request to acquire savings");
                var result=await _progressService.gettingSavings(gid);
                if(result==null)
                {
                    _logger.LogWarning("No savings found for Goal: {Gid}", gid);
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
