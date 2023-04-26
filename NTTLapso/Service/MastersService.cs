using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.Permissions;
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

        public  Task<int> PermissionRegister(PermissionRequest permissionRequest)
        {
            return  _repo.PermissionRegister(permissionRequest);
        }

        public Task<PermissionDataResponse> GetPermission(PermissionRequest permissionRequest)
        {
            return _repo.GetPermission(permissionRequest);
        }

        public Task<int> DeletePermission(PermissionRequest permissionRequest)
        {
            return _repo.DeletePermission(permissionRequest);
        }

        public Task<int> UpdatePermission(PermissionRequest permissionRequest)
        {
            return _repo.UpdatePermission(permissionRequest);
        }
    }
}
