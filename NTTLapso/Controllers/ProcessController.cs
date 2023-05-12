using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.General;
using NTTLapso.Models.Process.UserCharge;
using NTTLapso.Service;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("Process")]
    public class ProcessController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<ProcessController> _logger;
        private ProcessService _service;
        public ProcessController(ILogger<ProcessController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new ProcessService(_config);
        }

        // Method for inserting user's vacation and compensated days at beginning of year.
        [HttpPost]
        [Route("UserCharge/SetUsersCharge")]
        [AllowAnonymous]
        public async Task<UserChargeResponse> SetUsersCharge()
        {
            UserChargeResponse response = new UserChargeResponse();

            try
            {
                await _service.SetUsersCharge();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }
    }
}
