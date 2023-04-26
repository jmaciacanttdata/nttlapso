using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.Permissions;
using NTTLapso.Service;
using System.Diagnostics;
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
        [Route("Permission")]
        public async Task<ActionResult> PermissionRegister(PermissionRequest request)
        {
            int permissionData = await _service.PermissionRegister(request);
            if (permissionData != 0)
            {
                PermissionResponse response = new PermissionResponse();
                response.IsRegistered = true;
                return Ok(response);
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("Fallo en la respuesta del servidor");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("Compruebe que ha introducido todos los datos correctamente");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpGet]
        [Route("GetPermission")]
        public async Task<ActionResult> GetPermission([FromQuery]PermissionRequest request)
        {
           
            PermissionDataResponse response = await _service.GetPermission(request);
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
                if(BadRequest().StatusCode == 500)
                {
                    return BadRequest("Fallo en la respuesta del servidor");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("Compruebe que ha introducido todos los datos correctamente");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult> DeletePermission(PermissionRequest request)
        {
            int permissionData = await _service.DeletePermission(request);
            if (permissionData != 0)
            {
                PermissionResponse response = new PermissionResponse();
                response.IsRegistered = true;
                return Ok(response);
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("Fallo en la respuesta del servidor");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("Compruebe que ha introducido todos los datos correctamente");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> UpdatePermission(PermissionRequest request)
        {
            int permissionData = await _service.UpdatePermission(request);
            if (permissionData != 0)
            {
                PermissionResponse response = new PermissionResponse();
                response.IsRegistered = true;
                return Ok(response); // completed successfully
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("Fallo en la respuesta del servidor");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("Compruebe que ha introducido todos los datos correctamente");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

    }

}
