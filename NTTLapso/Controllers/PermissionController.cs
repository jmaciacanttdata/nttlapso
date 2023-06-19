using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.General;
using NTTLapso.Models.Permissions;
using NTTLapso.Service;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("NTTLapso/Permission")]
    public class PermissionController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PermissionController> _logger;
        private PermissionService _service;
        public PermissionController(ILogger<PermissionController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new PermissionService(_config);
        }

        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<ListPermissions> List(PermissionDataResponse? request)
        {
            ListPermissions response = new ListPermissions();
            List<PermissionDataResponse> responseList = new List<PermissionDataResponse>();
            try
            {
                responseList = await _service.List(request);
                response.IsSuccess = true;
                response.Data = responseList;
                response.Error = null;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;
            }

            return response;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<PermissionResponse> Create(CreatePermissionRequest request)
        {
            PermissionResponse response = new PermissionResponse();

            try
            {
                if(request.Value != null && request.Value != "")
                {
                    await _service.Create(request);
                    response.IsSuccess = true;
                }
                else
                {
                    Error _error = new Error(" The Value field cant be empty ",null);
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

        [HttpPost]
        [Route("Edit")]
        [Authorize]
        public async Task<PermissionResponse> Edit(EditPermissionRequest request)
        {
            PermissionResponse response = new PermissionResponse();

            try
            {
                await _service.Edit(request);
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

        [HttpGet]
        [Route("Delete")]
        [Authorize]
        public async Task<PermissionResponse> Delete(int Id)
        {
            PermissionResponse response = new PermissionResponse();

            try
            {
                await _service.Delete(Id);
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
