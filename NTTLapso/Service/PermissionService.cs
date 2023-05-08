using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.Permissions;
using NTTLapso.Models.PetitionStatus;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class PermissionService
    {
        private PermissionRepository _repo = new PermissionRepository();
        public PermissionService() { }

        //Used to send to the login token a list of permissions for each user team
        public async Task<List<LoginUserPermissionResponse>> ListUserPermission(int userId)
        {
            return await _repo.ListUserPermission(userId);
        }
        public async Task<List<PermissionDataResponse>> List(PermissionDataResponse request)
        {
            return await _repo.List(request);
        }

        public async Task Create(CreatePermissionRequest request)
        {
            await _repo.Create(request);
        }

        public async Task Edit(EditPermissionRequest request)
        {
            await _repo.Edit(request);
        }

        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
    }
}
