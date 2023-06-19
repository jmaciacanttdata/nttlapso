using Microsoft.Extensions.Configuration;
using NTTLapso.Models.Enum;
using NTTLapso.Models.General;
using NTTLapso.Models.Mail;
using NTTLapso.Models.Process.EmailConfig;
using NTTLapso.Models.Process.UserCharge;
using NTTLapso.Models.Users;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class ProcessService
    {
        private IConfiguration _config;
        private ProcessRepository _repo;
        public ProcessService(IConfiguration conf) {
            _config = conf;
            _repo = new ProcessRepository(_config);
        }

        // Method for inserting user's vacation and compensated days at beginning of year.
        public async Task SetUsersCharge()
        {
            int compensatedDays = _config.GetValue<int>("UserCharge:CompensatedDays");
            int vacationDays = _config.GetValue<int>("UserCharge:VacationDays");
            UserRepository userRepository = new UserRepository(_config);
            List<UserDataResponse> users = userRepository.List(new UserListRequest { }).Result;
            int year = DateTime.Now.Year;

            if(users.Count > 0) // Check if users in data base.
            {
                foreach (var user in users)
                {
                    UserChargeRequest request = new UserChargeRequest();
                    request.IdUser = user.Id;
                    request.Year = year;
                    request.TotalVacationDays = vacationDays;

                    if (user.Schedule.Id == 1)
                    {
                        request.TotalCompensatedDays = 0;
                        await _repo.SetUsersCharge(request);
                    }
                    else if (user.Schedule.Id == 2)
                    {
                        request.TotalCompensatedDays = compensatedDays;
                        await _repo.SetUsersCharge(request);
                    }
                }
            }
            else
            {
                throw new Exception(message: "No users available in data base");
            }
        }

        // Method for inserting new user's vacation and compensated days at registration date.
        public async Task SetNewUserCharge(NewUserChargeRequest newUserChargeRequest)
        {
            int compensatedDays = _config.GetValue<int>("UserCharge:CompensatedDays");
            int vacationDays = _config.GetValue<int>("UserCharge:VacationDays");
            int year = DateTime.Now.Year;
            
            UserChargeRequest request = new UserChargeRequest();
            request.IdUser = newUserChargeRequest.IdUser;
            request.Year = year;
            request.TotalVacationDays = CheckDaysNewUser(newUserChargeRequest.RegisterDate, vacationDays);

            if (newUserChargeRequest.IdSchedule == 1)
            {
                request.TotalCompensatedDays = 0;
                await _repo.SetNewUserCharge(request);
            }
            else if (newUserChargeRequest.IdSchedule == 2)
            {
                request.TotalCompensatedDays = CheckDaysNewUser(newUserChargeRequest.RegisterDate, compensatedDays);
                await _repo.SetNewUserCharge(request);
            }
        }

        // Method that checks how many days (Vacation, compensated ...) you get for the remainder of the year.
        private int CheckDaysNewUser(DateTime registerDate, int totalDays)
        {
            if (registerDate.Year == DateTime.Now.Year ) //  Check if register date is fom current year.
            {
                //Get total vacations day per day
                float totalVacationsPerDay = ((float)totalDays / 12)/30;
                int totalDaysLabor = 0;

                //Get month diff from now to end of year
                int monthDiff = 12 - registerDate.Month;

                //Get total days labor from now to end of year
                if (monthDiff > 0)
                {
                    totalDaysLabor += monthDiff * 30;
                }
                totalDaysLabor += 30 - registerDate.Day;

                //Calculate the vacations day from now to end of year
                float totalVacationsDayUser = totalDaysLabor * totalVacationsPerDay;

                // Days for the remaining of the year.
                int result = (int)Math.Round(totalVacationsDayUser);

                return result;
            }
            else
            {
                throw new Exception(message: "The date has to match current year");
            }
        }

        internal async Task SendNotification(MailSender sender)
        {
            try
            {
                await _repo.SendNotification(sender);
            }
            catch (Exception e)
            {
                throw new Exception(message: e.Message);
            }
        }
    }
}
