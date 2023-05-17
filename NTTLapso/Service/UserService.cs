using NTTLapso.Models.Permissions;
using NTTLapso.Models.Process.UserCharge;
using NTTLapso.Models.Users;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class UserService
    {
        private UserRepository _repo = new UserRepository();
        public UserService() { }

        public async Task<List<UserDataResponse>> List(UserListRequest request)
        {
            return await _repo.List(request);
        }
        public async Task<NewUserChargeRequest> Create(CreateUserRequest request)
        {
            return await _repo.Create(request);
        }
        public async Task Edit(EditUserRequest request)
        {
            await _repo.Edit(request);
        }
        public async Task ChangeUserState(ChangeUserStateRequest request)
        {
            await _repo.ChangeUserState(request);
        }
        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
        internal async Task SetUserTeam(UserTeamRequest request)
        {
            await _repo.SetUserTeam(request);
        }
    }
}
