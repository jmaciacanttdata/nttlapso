using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.General;
using NTTLapso.Models.Process.UserCharge;
using NTTLapso.Models.Users;
using NTTLapso.Service;


namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;
        private UserService _service = new UserService();
        private ProcessService _serviceProcess; 
        public UserController(ILogger<UserController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _serviceProcess = new ProcessService(_config);
        }
        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<ListUsers> List(UserDataResponse? request)
        {
            ListUsers response = new ListUsers();
            List<UserDataResponse> responseList = new List<UserDataResponse>();
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
        public async Task<UserResponse> Create(CreateUserRequest request)
        {
            UserResponse response = new UserResponse();
            NewUserChargeRequest userCharge = new NewUserChargeRequest();

            try
            {

                    userCharge = await _service.Create(request);
                    await _serviceProcess.SetNewUserCharge(userCharge);
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
        public async Task<UserResponse> Edit(EditUserRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {
                await _service.Edit(request);
                response.IsSuccess = true;
            }
            catch(Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;
            }
            return response;
        }

        [HttpPost]
        [Route("ChangeUserState")]
        [Authorize]
        public async Task<UserResponse> ChangeUserState(ChangeUserStateRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {

                await _service.ChangeUserState(request);
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
        public async Task<UserResponse> Delete(int Id)
        {
            UserResponse response = new UserResponse();

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
