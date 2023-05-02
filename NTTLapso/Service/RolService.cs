using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;
using NTTLapso.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Service
{
    public class RolService
    {
        private RolRepository _repo = new RolRepository();
        public RolService() { }

        public async Task<List<IdValue>> List(IdValue request) {
            return await _repo.List(request);
        }

        public async Task Create(string value)
        {
            await _repo.Create(value);
        }

        public async Task Edit(IdValue request)
        {
            await _repo.Edit(request);
        }

        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
    }

}
