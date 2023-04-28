using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.RolPermission;
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

        // Register a new rol with it's permissions.
        internal async Task<ErrorResponse> SetRolPermission(SetRolPermissionRequest request)
        {
            return await _repo.SetRolPermission(request);
        }

        // Get a list of rols with it's permissions.
        public async Task<GetRolPermissionResponse> GetRolsPermissionList()
        {
            return await _repo.GetRolsPermissionList();
        }

        // Update rol permissions.
        public async Task<ErrorResponse> UpdateRolPermission(SetRolPermissionRequest request)
        {
            return await _repo.UpdateRolPermission(request);
        }

        // Delete rol permissions.
        internal async Task<ErrorResponse> DeleteRolPermissions(int rolId)
        {
            return await _repo.DeleteRolPermissions(rolId);
        }
    }
}
