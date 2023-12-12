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
    public class PetitionStateService
    {
        private PetitionStateRepository _repo;
        private IConfiguration _configuration;
        public PetitionStateService(IConfiguration config) 
        {
            _configuration = config;
            _repo = new PetitionStateRepository(config);
        }

        public async Task<List<PetitionStatusDataResponse>> List()
        {
            return await _repo.List();
        }

        public async Task Create(CreatePetitionStatusRequest request)
        {
            await _repo.Create(request);
        }

        public async Task Edit(EditPetitionStatusRequest request)
        {
            await _repo.Edit(request);
        }

        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
    }

}
