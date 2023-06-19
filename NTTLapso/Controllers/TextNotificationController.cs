using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.TextNotification;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using NTTLapso.Models.Rol;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("NTTLapso/TextNotification")]
    public class TextNotificationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TextNotificationController> _logger;
        private TextNotificationService _service;
        public TextNotificationController(ILogger<TextNotificationController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new TextNotificationService(_config);
        }


        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<ListTextNotificationResponse> List(IdTextNotificationRequest? request)
        {
            ListTextNotificationResponse response = new ListTextNotificationResponse();
            List<TextNotificationData> responseList = new List<TextNotificationData>();
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
        public async Task<TextNotificationResponse> Create(TextNotificationRequest request)
        {
            TextNotificationResponse response = new TextNotificationResponse();

            try
            {
                await _service.Create(request);
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

        [HttpPost]
        [Route("Edit")]
        [Authorize]
        public async Task<TextNotificationResponse> Edit(IdTextNotificationRequest request)
        {
            TextNotificationResponse response = new TextNotificationResponse();

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
        public async Task<TextNotificationResponse> Delete(int Id)
        {
            TextNotificationResponse response = new TextNotificationResponse();

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
