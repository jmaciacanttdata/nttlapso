using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionType;
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

        // Register a new petition type.
        [Route("PetitionTypeRegister")]
        [HttpPost]
        public async Task<IActionResult> PetitionTypeRegister([FromBody] PetitionTypeRegisterRequest request)
        {
            ErrorResponse response = await _service.PetitionTypeRegister(request);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        // Get a petition type.
        [Route("GetPetitionType")]
        [HttpGet]
        public async Task<IActionResult> GetPetitionType([FromQuery] int petitionTypeId)
        {
            GetPetitionTypeResponse response = await _service.GetPetitionType(petitionTypeId);

            if (!response.Error.IsSuccess)
            {
                return BadRequest(response.Error);
            }
            else
            {
                return Ok(response.Data);
            }
        }

        // Get a list of petition types.
        [Route("GetPetitionTypeList")]
        [HttpGet]
        public async Task<IActionResult> GetPetitionTypeList()
        {
            GetPetitionTypeListResponse response = await _service.GetPetitionTypeList();

            if (!response.Error.IsSuccess)
            {
                return BadRequest(response.Error);
            }
            else
            {
                return Ok(response.Data);
            }
        }

        // Update a petition type.
        [Route("UpdatePetitionType")]
        [HttpPut]
        public async Task<IActionResult> UpdatePetitionType([FromBody] UpdatePetitionTypeRequest request)
        {
            ErrorResponse response = await _service.UpdatePetitionType(request);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        // Delete a petition type.
        [Route("DeletePetitionType")]
        [HttpDelete]
        public async Task<IActionResult> DeletePetitionType([FromQuery] int petitionTypeId)
        {
            ErrorResponse response = await _service.DeletePetitionType(petitionTypeId);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
