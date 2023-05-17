using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.General;
using NTTLapso.Models.UserSchedule;
using NTTLapso.Service;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("NTTLapso/UserSchedule")]
    public class UserScheduleController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserScheduleController> _logger;
        private UserScheduleService _service = new UserScheduleService();
        public UserScheduleController(ILogger<UserScheduleController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }


        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<ListUserScheduleResponse> List(IdValue? request)
        {
            ListUserScheduleResponse response = new ListUserScheduleResponse();
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
        [Authorize]
        public async Task<UserScheduleResponse> Create(string value)
        {
            UserScheduleResponse response = new UserScheduleResponse();

            try
            {
                if(value != null && value != "")
                {
                    await _service.Create(value);
                    response.IsSuccess = true;
                }
                else
                {
                    Error _error = new Error("The value field cant be empty",null);
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
        public async Task<UserScheduleResponse> Edit(IdValue request)
        {
            UserScheduleResponse response = new UserScheduleResponse();

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
        public async Task<UserScheduleResponse> Delete(int Id)
        {
            UserScheduleResponse response = new UserScheduleResponse();

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
