using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.Rols;
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

        // Register a new rol.
        [Route("RolRegister")]
        [HttpPost]
        public async Task<IActionResult> RolRegisterAsync([FromBody] RolRegisterRequest request)
        {
            RolRegisterResponse response = await _service.RolRegister(request);

            if (!response.IsRegistered)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        // Get a list of rols.
        [Route("GetRolsList")]
        [HttpGet]
        public async Task<IActionResult> GetRolsList()
        {
            GetRolsListResponse response = await _service.GetRolsList();

            if (response.ErrorType != null)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        // Update a rol.
        [Route("UpdateRol")]
        [HttpPut]
        public async Task<IActionResult> UpdateRol([FromBody] UpdateRolRequest request)
        {
            UpdateRolResponse response = await _service.UpdateRol(request);

            if (!response.IsUpdated)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        // Delete a rol.
        [Route("DeleteRol")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRol([FromQuery] int rolId)
        {
            UpdateRolResponse response = await _service.DeleteRol(rolId);

            if (!response.IsUpdated)
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
