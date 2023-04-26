using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.Rols;
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

        // Register a new rol.
        internal async Task<RolRegisterResponse> RolRegister(RolRegisterRequest request)
        {
            return await _repo.RolRegister(request);
        }

        // Get a list of rols.
        internal async Task<GetRolsListResponse> GetRolsList()
        {
            return await _repo.GetRolsList();
        }

        // Get a list of rols.
        internal async Task<UpdateRolResponse> UpdateRol(UpdateRolRequest request)
        {
            return await _repo.UpdateRol(request);
        }

        // Delete a rol.
        internal async Task<UpdateRolResponse> DeleteRol(int rolId)
        {
            return await _repo.DeleteRol(rolId);
        }
    }
}
