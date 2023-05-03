using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Category;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.Rol;
using NTTLapso.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("Rol")]
    public class RolController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<RolController> _logger;
        private RolService _service = new RolService();
        public RolController(ILogger<RolController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }


        [HttpPost]
        [Route("List")]
        [AllowAnonymous]
        public async Task<ListRolResponse> List(IdValue? request) {
            ListRolResponse response = new ListRolResponse();
            List<IdValue> responseList = new List<IdValue>();
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
        [AllowAnonymous]
        public async Task<RolResponse> Create(string value)
        {
            RolResponse response = new RolResponse();

            try
            {
                await _service.Create(value);
                response.IsSuccess = true;
            }catch(Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        [HttpPost]
        [Route("Edit")]
        [AllowAnonymous]
        public async Task<RolResponse> Edit(IdValue request)
        {
            RolResponse response = new RolResponse();

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
        [AllowAnonymous]
        public async Task<RolResponse> Delete(int Id)
        {
            RolResponse response = new RolResponse();

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
