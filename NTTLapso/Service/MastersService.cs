using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.UserSchedule;
using NTTLapso.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Service
{
    public class MastersService
    {
        private MastersRepository _repo = new MastersRepository();
        public MastersService() { }

        public Task<int> SetUserSchedule(UserScheduleSetRequest userScheduleRequest)
        {
            return _repo.SetUserSchedule(userScheduleRequest);
        }

        public Task<UserScheduleDataResponse> GetUserSchedule(UserScheduleRequest userScheduleRequest)
        {
            return _repo.GetUserSchedule(userScheduleRequest);
        }

        public Task<int> DeleteUserSchedule(UserScheduleRequest userScheduleRequest)
        {
            return _repo.DeleteUserSchedule(userScheduleRequest);
        }

        public Task<int> UpdateUserSchedule(UserScheduleRequest userScheduleRequest)
        {
            return _repo.UpdateUserSchedule(userScheduleRequest);
        }
    }
}
