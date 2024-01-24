using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
    [Route("NTTLapso/User")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;
        private UserService _userService;
        private TeamService _teamService;
        private ProcessService _processService; 
        private TextNotificationService _textNotificationService;
        public UserController(ILogger<UserController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _userService = new UserService(_config);
            _teamService = new TeamService(_config);
            _textNotificationService = new TextNotificationService(_config);
            _processService = new ProcessService(_config);
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
                responseList = await _userService.List(request);
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
        [Route("GetUserRank")]
        [Authorize]
        public async Task<ActionResult> GetUserRank(UserListRequest user)
        {
            var response = await _userService.GetUserRank(user);
            return response.Completed ? Ok(response) : StatusCode(response.StatusCode, response);
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

            try
            {
                userCharge = await _userService.Create(request);
                List<TeamManagerDataResponse> managerList = await _teamService.GetTeamsManagerList(request.IdTeam, 0);

                //Conformamos el objeto sender para el envío de la notificación.
                sender.Receiver.Id = userCharge.IdUser;
                sender.Receiver.Name = request.Name;
                sender.Receiver.Email = request.Email;
                UserMail receiverCC = new UserMail()
                {
                    Id = managerList[0].Id,
                    Name = managerList[0].Name,
                    Email = managerList[0].Email,
                };
                sender.ReceiverCCList.Add(receiverCC);
                List<TextNotificationData> notification = await _textNotificationService.List(new IdTextNotificationRequest() {
                    Id = (int)NotificationType.SendNotificationOfNewUserRegister
                }); 
                sender.Content.Subject = notification[0].Subject;
                sender.Content.Content = notification[0].Content;
                sender.Content.IdNotificationType = notification[0].IdNotification;
                MailReplacer replacer1 = new MailReplacer();
                replacer1.SearchText = "{{user_name}}";
                replacer1.ReplaceText = request.Name;
                MailReplacer replacer2 = new MailReplacer();
                replacer2.SearchText = "{{manager_name}}";
                replacer2.ReplaceText = managerList[0].Name;
                replacerList.Add(replacer1);
                replacerList.Add(replacer2);
                sender.Replacers = replacerList;

                await _processService.SendNotification(sender);
                await _processService.SetNewUserCharge(userCharge);
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
                await _userService.Edit(request);
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
            List<TeamManagerDataResponse> managerList = await _teamService.GetTeamsManagerList(0, request.Id);
            MailSender sender = new MailSender();
            List<MailReplacer> replacerList = new List<MailReplacer>();

            try
            {
                await _userService.ChangeUserState(request);

                //Conformamos el objeto sender para el envío de la notificación (email).
                UserDataResponse user = (await _userService.List(new UserListRequest() { Id = request.Id })).First();
                sender.Receiver.Id = (int)user.Id;
                sender.Receiver.Name = user.Name;
                sender.Receiver.Email = user.Email;
                foreach (var manager in managerList)
                {
                    UserMail receiverCC = new UserMail()
                    {
                        Id = manager.Id,
                        Name = manager.Name,
                        Email = manager.Email,
                    };
                    sender.ReceiverCCList.Add(receiverCC);
                }
                List<TextNotificationData> notification = await _textNotificationService.List(new IdTextNotificationRequest()
                {
                    Id = request.Active ? (int)NotificationType.SendNotificationOfNewUserRegister : (int)NotificationType.SendNotificationOfUnRegister
                });
                sender.Content.Subject = notification[0].Subject;
                sender.Content.Content = notification[0].Content;
                sender.Content.IdNotificationType = notification[0].IdNotification;
                MailReplacer replacer1 = new MailReplacer();
                replacer1.SearchText = "{{user_name}}";
                replacer1.ReplaceText = user.Name;
                MailReplacer replacer2 = new MailReplacer();
                replacer2.SearchText = "{{manager_name}}";
                replacer2.ReplaceText = request.NameApprover;
                replacerList.Add(replacer1);
                replacerList.Add(replacer2);
                sender.Replacers = replacerList;

                await _processService.SendNotification(sender);
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
                await _userService.Delete(Id);
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
        [Route("SetUserTeam")]
        [Authorize]
        public async Task<UserResponse> SetUserTeam(UserTeamRequest request)
        {
            UserResponse response = new UserResponse();
            try
            {
                await _userService.SetUserTeam(request);
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
