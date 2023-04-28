using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.RolPermission;
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

        // Register a new rol with it's permissions.
        [Route("SetRolPermission")]
        [HttpPost]
        public async Task<IActionResult> SetRolPermission([FromBody] SetRolPermissionRequest request)
        {
            ErrorResponse response = await _service.SetRolPermission(request);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }
        
        // Get a list of rols with it's permissions.
        [Route("GetRolsPermissionList")]
        [HttpGet]
        public async Task<IActionResult> GetRolsPermissionList()
        {
            GetRolPermissionResponse response = await _service.GetRolsPermissionList();

            if (!response.Error.IsSuccess)
            {
                return BadRequest(response.Error);
            }
            else
            {
                return Ok(response.Data);
            }
        }

        // Update rol permissions.
        [Route("UpdateRolPermission")]
        [HttpPut]
        public async Task<IActionResult> UpdateRolPermission([FromBody] SetRolPermissionRequest request)
        {
            ErrorResponse response = await _service.UpdateRolPermission(request);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        // Delete rol permissions.
        [Route("DeleteRolPermissions")]
        [HttpDelete]
        public async Task<IActionResult> DeleteRolPermissions([FromQuery] int rolId)
        {
            ErrorResponse response = await _service.DeleteRolPermissions(rolId);

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
