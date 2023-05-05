﻿using Microsoft.AspNetCore.Authorization;
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
        private ProcessService _service = new ProcessService();
        public ProcessController(ILogger<ProcessController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
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
                await _service.SetUsersCharge(this._config);
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

        // Method for inserting new user's vacation and compensated days at registration date.
        [HttpPost]
        [Route("UserCharge/SetNewUserCharge")]
        [AllowAnonymous]
        public async Task<UserChargeResponse> SetNewUserCharge(NewUserChargeRequest newUserChargeRequest)
        {
            UserChargeResponse response = new UserChargeResponse();

            try
            {
                await _service.SetNewUserCharge(this._config, newUserChargeRequest);
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