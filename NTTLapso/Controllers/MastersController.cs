using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;
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
        [Route("PetitionStatus")]
        public async Task<ActionResult> PetitionRegister(PetitionStatusSetRequest petitionStatusRequest)
        {
            int petitionData = await _service.PetitionRegister(petitionStatusRequest);
            if (petitionData != 0)
            {
                PetitionStatusResponse response = new PetitionStatusResponse();
                response.IsSucces = true;
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

        [HttpGet]
        [Route("GetPetitionStatus")]
        public async Task<ActionResult> GetPetitionStatus([FromQuery] PetitionStatusRequest petitionStatusRequest)
        {

            PetitionStatusDataResponse response = await _service.GetPetitionStatus(petitionStatusRequest);
            if (response.Id != 0)
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
        [Route("DeletePetitionStatus")]
        public async Task<ActionResult> DeletePetitionStatus(PetitionStatusRequest petitionStatusRequest)
        {
            int permissionData = await _service.DeletePetitionStatus(petitionStatusRequest);
            if (permissionData != 0)
            {
                PetitionStatusResponse response = new PetitionStatusResponse();
                response.IsSucces = true;
                return Ok(response + "completed successfully");
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
        [Route("UpdatePetitionStatus")]
        public async Task<ActionResult> UpdatePetitionStatus(PetitionStatusRequest petitionStatusRequest)
        {
            int petitionData = await _service.UpdatePetitionStatus(petitionStatusRequest);
            if (petitionData != 0)
            {
                PetitionStatusResponse response = new PetitionStatusResponse();
                response.IsSucces = true;
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
