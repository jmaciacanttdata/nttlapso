using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.TextNotification;
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
        [Route("SetTextNotification")]
        public async Task<ActionResult> SetTextNotification(SetTextNotificationRequest request)
        {
            TextNotificationResponse textNotificationData = await _service.SetTextNotification(request);
            if (textNotificationData.isSuccess == true)
            {
                return Ok("OKAY");
            }
            else
            {    
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("The server is not responding, check your connection and try again");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("The data entered does not exist or is incorrect, please review it and try again");
                }
                else 
                { 
                    return BadRequest(); 
                }
            }
        }

        [HttpGet]
        [Route("GetTextNotification")]
        public async Task<ActionResult> GetTextNotification([FromQuery]TextNotificationRequest request)
        {
            TextNotificationDataResponse textNotificationData = await _service.GetTextNotification(request);
            if (textNotificationData != null)
            {
                return Ok(textNotificationData);
            }
            else
            {
                if(BadRequest().StatusCode == 500)
                {
                    return BadRequest("The server is not responding, check your connection and try again");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("The data entered does not exist or is incorrect, please review it and try again");
                }
                else { return BadRequest(); }
            }
        }

        [HttpPut]
        [Route("UpdateTextNotification")]
        public async Task<ActionResult> UpdateTextNotification(TextNotificationRequest request)
        {
            TextNotificationResponse textNotificationData = await _service.UpdateTextNotification(request);
            if (textNotificationData.isSuccess == true)
            {
                return Ok("OKAY");
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("The server is not responding, check your connection and try again");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("The data entered does not exist or is incorrect, please review it and try again");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpDelete]
        [Route("DeleteTextNotification")]
        public async Task<ActionResult> DeleteTextNotification(TextNotificationRequest request)
        {
            TextNotificationResponse textNotificationData = await _service.DeleteTextNotification(request);
            if (textNotificationData.isSuccess == true)
            {
                return Ok("OKAY");
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("The server is not responding, check your connection and try again");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("The data entered does not exist or is incorrect, please review it and try again");
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
