using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.General;
using NTTLapso.Models.Rol;
using NTTLapso.Models.RolPermission;
using NTTLapso.Service;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("NTTLapso/RolPermission")]
    public class RolPermissionController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<RolPermissionController> _logger;
        private RolPermissionService _service;
        public RolPermissionController(ILogger<RolPermissionController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new RolPermissionService(_config);
        }

        // Get a list of rols with it's permissions.
        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<RolPermissionListResponse> List([FromBody]int? idRol)
        {
            RolPermissionListResponse response = new RolPermissionListResponse();
            List<RolPermissionDataResponse> responseList = new List<RolPermissionDataResponse>();
            try
            {
                if(idRol != null)
                {
                    responseList = await _service.List(idRol);
                    response.IsSuccess = true;
                    response.Data = responseList;
                    response.Error = null;
                }
                else
                {
                    Error _error = new Error("The idRol request data is required", null);
                    response.IsSuccess = false;
                    response.Error = _error;
                }
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;
            }

            return response;
        }

        // Insert a new rol with permissions.
        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<RolPermissionResponse> Create(RolPermissionRequest request)
        {
            RolPermissionResponse response = new RolPermissionResponse();

            try
            {
                if(request != null && request.PermissionList.Count() != 0)
                {
                    await _service.Create(request);
                    response.IsSuccess = true;
                }
                else
                {
                    Error error = new Error("Permission list can't be empty.", null);
                    response.IsSuccess = false;
                    response.Error = error;
                }
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        // Update permissions from a rol.
        [HttpPost]
        [Route("Edit")]
        [Authorize]
        public async Task<RolPermissionResponse> Edit(RolPermissionRequest request)
        {
            RolPermissionResponse response = new RolPermissionResponse();

            try
            {
                if (request != null && request.PermissionList.Count() != 0)
                {
                    await _service.Edit(request);
                    response.IsSuccess = true;
                }
                else
                {
                    Error error = new Error("Permission list can't be empty.", null);
                    response.IsSuccess = false;
                    response.Error = error;
                }
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        // Delete rol and it's permissions.
        [HttpGet]
        [Route("Delete")]
        [Authorize]
        public async Task<RolPermissionResponse> Delete( int idRol)
        {
            RolPermissionResponse response = new RolPermissionResponse();

            try
            {
                await _service.Delete(idRol);
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
