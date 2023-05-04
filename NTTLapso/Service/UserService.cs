﻿using NTTLapso.Models.Permissions;
using NTTLapso.Models.Users;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class UserService
    {
        private UserRepository _repo = new UserRepository();
        public UserService() { }

        public async Task<List<UserDataResponse>> List(UserDataResponse request)
        {
            return await _repo.List(request);
        }
        public async Task Create(CreateUserRequest request)
        {
            await _repo.Create(request);
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
    }
}
