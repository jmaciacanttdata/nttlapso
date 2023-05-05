using NTTLapso.Models.General;
using NTTLapso.Models.Process.UserCharge;
using NTTLapso.Models.Users;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class ProcessService
    {
        private ProcessRepository _repo = new ProcessRepository();
        public ProcessService() { }

        // Method for inserting user's vacation and compensated days at beginning of year.
        public async Task SetUsersCharge(IConfiguration _config)
        {
            int compensatedDays = _config.GetValue<int>("UserCharge:CompensatedDays");
            int vacationDays = _config.GetValue<int>("UserCharge:VacationDays");
            UserRepository userRepository = new UserRepository();
            List<UserDataResponse> users = userRepository.List(new UserDataResponse { }).Result;
            int year = DateTime.Now.Year;

            if(users.Count > 0) // Check if users in data base.
            {
                foreach (var user in users)
                {
                    UserChargeRequest request = new UserChargeRequest();
                    request.IdUser = user.Id;
                    request.Year = year;
                    request.TotalVacationDays = vacationDays;

                    if (user.Schedule == "Extendido")
                    {
                        request.TotalCompensatedDays = compensatedDays;
                        await _repo.SetUsersCharge(request);
                    }
                    else if (user.Schedule == "General")
                    {
                        request.TotalCompensatedDays = 0;
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
        public async Task SetNewUserCharge(IConfiguration _config, NewUserChargeRequest newUserChargeRequest)
        {
            int compensatedDays = _config.GetValue<int>("UserCharge:CompensatedDays");
            int vacationDays = _config.GetValue<int>("UserCharge:VacationDays");
            int year = DateTime.Now.Year;
            
            UserChargeRequest request = new UserChargeRequest();
            request.IdUser = newUserChargeRequest.IdUser;
            request.Year = year;
            request.TotalVacationDays = CheckDaysNewUser(newUserChargeRequest.RegisterDate, vacationDays);

            UserScheduleRepository repository = new UserScheduleRepository();
            IdValue schedule = (await repository.List(new IdValue() { Id = newUserChargeRequest.IdSchedule, Value = "" })).FirstOrDefault();

            if(schedule != default) // Check if schedule exists in data base.
            {
                if (schedule.Value == "Extendido")
                {
                    request.TotalCompensatedDays = CheckDaysNewUser(newUserChargeRequest.RegisterDate, compensatedDays);
                    await _repo.SetNewUserCharge(request);
                }
                else if (schedule.Value == "General")
                {
                    request.TotalCompensatedDays = 0;
                    await _repo.SetNewUserCharge(request);
                }
            }
            else
            {
                throw new Exception(message: $"There are no schedules in database with id: {newUserChargeRequest.IdSchedule}");
            }
        }

        // Method that checks how many days (Vacation, compensated ...) you get for the remainder of the year.
        private int CheckDaysNewUser(DateTime registerDate, int totalDays)
        {
            if (registerDate.Year == DateTime.Now.Year ) //  Check if register date is fom current year.
            {
                // Get remaining days of year.
                DateTime endOfYear = new DateTime(registerDate.Year, 12, 31);
                TimeSpan remainingDays = endOfYear - registerDate;
                int daysLeft = remainingDays.Days;

                // Divide totalDays by months / days of month.
                decimal totalDaysPerDay = ((decimal)totalDays / 12) / 30;

                // Days for the remaining of the year.
                int result = (int)Math.Round(totalDaysPerDay * daysLeft);

                return result;
            }
            else
            {
                throw new Exception(message: "The date has to match current year");
            }
        }
    }
}
