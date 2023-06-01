using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.Enum;
using NTTLapso.Models.General;
using NTTLapso.Models.Mail;
using NTTLapso.Models.Team;
using NTTLapso.Models.TextNotification;
using NTTLapso.Models.Users;
using NTTLapso.Models.Vacations;
using NTTLapso.Repository;
using NTTLapso.Service;
using System.Reflection;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("NTTLapso/Vacation")]
    public class VacationController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<VacationController> _logger;
        private VacationService _service;
        private UserService _userService;
        private TeamService _teamService;
        private ProcessService _processService; 
        private TextNotificationService _textNotificationService;
        public VacationRepository _repo;
        public VacationController(ILogger<VacationController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _processService = new ProcessService(_config);
            _service = new VacationService(_config);
            _userService = new UserService(_config);
            _teamService = new TeamService(_config);
            _textNotificationService = new TextNotificationService(_config);
            _repo = new VacationRepository(_config);
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<VacationResponse> Create(CreateVacationRequest request)
        {
            VacationResponse response = new VacationResponse();
            List<TeamManagerDataResponse> managerList = await _teamService.GetTeamsManagerList(0, request.IdUserPetition);
            MailSender sender = new MailSender();
            List<MailReplacer> replacerList = new List<MailReplacer>();

            try
            {
                if (request.IdUserPetition != 0)
                {
                    if (await _repo.CheckViability(request.IdUserPetition, request.Day))
                    {
                        await _service.Create(request);

                        //Conformamos el objeto sender para el envío de la notificación (email).
                        UserDataResponse user = (await _userService.List(new UserListRequest() { Id = request.IdUserPetition })).First();
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
                            Id = (int)NotificationType.SendNotificationOfNewVacationRequest
                        });
                        sender.Content.Subject = notification[0].Subject;
                        sender.Content.Content = notification[0].Content;
                        sender.Content.IdNotificationType = notification[0].IdNotification;
                        MailReplacer replacer1 = new MailReplacer();
                        replacer1.SearchText = "{{user_name}}";
                        replacer1.ReplaceText = user.Name + " " + user.Surnames;
                        replacerList.Add(replacer1);
                        sender.Replacers = replacerList;

                        await _processService.SendNotification(sender);
                        response.IsSuccess = true;
                    }
                    else
                    {
                        Error _error = new Error(" The selected date isn´t available in your team ", null);
                        response.IsSuccess = false;
                        response.Error = _error;
                    }
                }
                else
                {
                    response.IsSuccess = false;
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
        public async Task<VacationResponse> Edit(EditVacationRequest request)
        {
            VacationResponse response = new VacationResponse();
            try
            {
                if (request.IdUserPetition != 0)
                {
                    if (await _repo.CheckViability(request.IdUserPetition, request.PetitionDate))
                    {
                        await _service.Edit(request);
                        response.IsSuccess = true;
                    }
                    else
                    {
                        Error _error = new Error(" The selected date isn´t available in your team ", null);
                        response.IsSuccess = false;
                        response.Error = _error;
                    }
                    //Llamar a sendAltaNotification()
                }
                else
                {
                    response.IsSuccess = false;
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

        // Delete vacation
        [HttpGet]
        [Route("Delete")]
        [Authorize]
        public async Task<VacationResponse> Delete(int IdVacation)
        {
            VacationResponse response = new VacationResponse();

            try
            {
                await _service.Delete(IdVacation);
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
        [Route("VacationApproved")]
        [Authorize]
        public async Task<VacationResponse> VacationApproved(VacationApprovedRequest request)
        {
            VacationResponse response = new VacationResponse();
            MailSender sender = new MailSender();
            List<MailReplacer> replacerList = new List<MailReplacer>();

            try
            {
                if (request.IdUserState != 0)
                {
                    await _service.VacationApproved(request);

                    List<TeamManagerDataResponse> managerList = await _teamService.GetTeamsManagerList(0, request.IdUserState);
                    UserDataResponse user = (await _userService.List(new UserListRequest() { Id = request.IdUserState })).First();

                    //Conformamos el objeto sender para el envío de la notificación.
                    sender.Receiver.Id = request.IdUserState;
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
                        Id = (int)NotificationType.SendNotificationApprovedVacations
                    });
                    sender.Content.Subject = notification[0].Subject;
                    sender.Content.Content = notification[0].Content;
                    sender.Content.IdNotificationType = notification[0].IdNotification;
                    MailReplacer replacer1 = new MailReplacer();
                    replacer1.SearchText = "{{user_name}}";
                    replacer1.ReplaceText = user.Name + " " + user.Surnames;
                    MailReplacer replacer2 = new MailReplacer();
                    replacer2.SearchText = "{{manager_name}}";
                    replacer2.ReplaceText = managerList[0].Name;
                    MailReplacer replacer3 = new MailReplacer();
                    replacer3.SearchText = "{{approved}}";
                    if (request.IdPetitionState == 2) {
                        replacer3.ReplaceText = "Approved";
                    } else if(request.IdPetitionState == 3)
                    {
                        replacer3.ReplaceText = "Denied";
                    }
                    MailReplacer replacer4 = new MailReplacer();
                    replacer4.SearchText = "{{details}}";
                    replacer4.ReplaceText = request.Detail == null ? request.Detail : "";
                    replacerList.Add(replacer1);
                    replacerList.Add(replacer2);
                    replacerList.Add(replacer3);
                    replacerList.Add(replacer4);
                    sender.Replacers = replacerList;

                    await _processService.SendNotification(sender);

                    response.IsSuccess = true;
                }
                else
                {
                    response.IsSuccess = false;
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

        // Get vacation state log list.
        [HttpPost]
        [Route("VacationStateLogList")]
        [Authorize]
        public async Task<VacationStateLogListResponse> VacationStateLogList(VacationStateLogListRequest? request)
        {
            VacationStateLogListResponse response = new VacationStateLogListResponse();
            List<VacationStateLogDataResponse> responseList = new List<VacationStateLogDataResponse>();
            try
            {
                responseList = await _service.VacationStateLogList(request);
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
        [Route("List")]
        [Authorize]
        public async Task<ListVacationResponse> List(ListVacationRequest request)
        {
            ListVacationResponse response = new ListVacationResponse();
            List<VacationData> responseList = new List<VacationData>();
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
        [Route("GetPercentagePetitionUserPerDayMonthList")]
        [Authorize]
        public async Task <PercentagePerMonthResponse> GetPercentagePetitionUserPerDayMonthList(PercentagePerMonthRequest request)
        {
            PercentagePerMonthResponse response = new PercentagePerMonthResponse();

            try
            {
                
                response.IsSuccess = true;
                response.Data = await _service.GetPercentagePetitionUserPerDayMonthList(request);
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
        [Route("Pendings")]
        [Authorize]
        public async Task<VacationPendingResponse> Pendings([FromQuery]int Id)
        {
            VacationPendingResponse response = new VacationPendingResponse();
            List<VacationPendingsData> responseList = new List<VacationPendingsData>();
            try
            {
                responseList = await _service.Pendings(Id);
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
    }
}
