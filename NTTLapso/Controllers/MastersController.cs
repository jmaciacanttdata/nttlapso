using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.UserSchedule;
using NTTLapso.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("Masters")]
    public class MastersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MastersController> _logger;
        private MastersService _service = new MastersService();
        public MastersController(ILogger<MastersController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpPost]
        [Route("SetUserSchedule")]
        public async Task<ActionResult> SetUserSchedule(UserScheduleSetRequest petitionStatusRequest)
        {
            int petitionData = await _service.SetUserSchedule(petitionStatusRequest);
            if (petitionData > 0)
            {
                UserScheduleResponse response = new UserScheduleResponse();
                response.IsSuccess = true;
                return Ok(response + " completado correctamente");
            }
            else if(petitionData == -2) 
            {
                return BadRequest("The entered data already exists");
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("Server response failed");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("Check that the data entered is correct");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet]
        [Route("GetUserSchedule")]
        public async Task<ActionResult> GetUserSchedule([FromQuery] UserScheduleRequest userScheduleRequest)
        {

            UserScheduleDataResponse response = await _service.GetUserSchedule(userScheduleRequest);
            if (response != null)
            {
                //    response.Value = request.Value;
                //    response.Registration = request.Registration;
                //    response.Read = request.Read;
                //    response.Edit = request.Edit;
                //    response.Delete = request.Delete;
                return Ok(response);
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("Server response failed");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("Check that the data entered is correct");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpDelete]
        [Route("DeleteUserSchedule")]
        public async Task<ActionResult> DeleteUserSchedule(UserScheduleRequest userScheduleRequest)
        {
            int userScheduleData = await _service.DeleteUserSchedule(userScheduleRequest);
            if (userScheduleData != 0)
            {
                UserScheduleResponse response = new UserScheduleResponse();
                response.IsSuccess = true;
                return Ok(response);
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("Server response failed");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("Check that the data entered is correct");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPut]
        [Route("UpdateUserSchedule")]
        public async Task<ActionResult> UpdateUserSchedule(UserScheduleRequest userScheduleRequest)
        {
            int userScheduleData = await _service.UpdateUserSchedule(userScheduleRequest);
            if (userScheduleData != 0)
            {
                UserScheduleResponse response = new UserScheduleResponse();
                response.IsSuccess = true;
                return Ok(response + "completed successfully"); // completed successfully
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("Server response failed");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("Check that the data entered is correct");
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
