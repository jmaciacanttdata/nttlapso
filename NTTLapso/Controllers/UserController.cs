using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.Enum;
using NTTLapso.Models.General;
using NTTLapso.Models.Mail;
using NTTLapso.Models.Process.UserCharge;
using NTTLapso.Models.Team;
using NTTLapso.Models.TextNotification;
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
        private UserService _service;
        private TeamService _teamService;
        private ProcessService _serviceProcess; 
        private TextNotificationService _serviceNotification = new TextNotificationService();
        public UserController(ILogger<UserController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new UserService();
            _teamService = new TeamService();
            _serviceProcess = new ProcessService(_config);
        }
        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<ListUsers> List(UserListRequest? request)
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
            MailSender sender = new MailSender();
            List<MailReplacer> replacerList = new List<MailReplacer>();
            GetTeamManagerResponse manager = new GetTeamManagerResponse();

            try
            {
                userCharge = await _service.Create(request);
                manager = await _teamService.GetTeamManager(request.IdTeam);

                //Conformamos el objeto sender para el envío de la notificación.
                sender.Receiver.Id = userCharge.IdUser;
                sender.Receiver.Name = request.Name;
                sender.Receiver.Email = request.Email;
                UserMail receiverCC = new UserMail();
                receiverCC.Id = manager.Id;
                receiverCC.Name = manager.Name;
                receiverCC.Email = manager.Email;
                sender.ReceiverCCList.Add(receiverCC);
                List<TextNotificationData> notification = await _serviceNotification.List(new IdTextNotificationRequest() {
                    Id = (int)NotificationType.SendNotificationOfNewUserRegisterToUser
                }); 
                sender.Content.Subject = notification[0].Subject;
                sender.Content.Content = notification[0].Content;
                sender.Content.IdNotificationType = notification[0].IdNotification;
                MailReplacer replacer1 = new MailReplacer();
                replacer1.SearchText = "{{user_name}}";
                replacer1.ReplaceText = request.Name + " " + request.Surnames;
                MailReplacer replacer2 = new MailReplacer();
                replacer2.SearchText = "{{manager_name}}";
                replacer2.ReplaceText = manager.Name + " " + manager.Surnames;
                replacerList.Add(replacer1);
                replacerList.Add(replacer2);
                sender.Replacers = replacerList;

                await _serviceProcess.SendNotification(sender);
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
