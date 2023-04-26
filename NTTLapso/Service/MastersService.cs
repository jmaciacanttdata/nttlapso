using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;
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

        public Task<int> PetitionRegister(PetitionStatusSetRequest petitionStatusRequest)
        {
            return _repo.PetitionRegister(petitionStatusRequest);
        }

        public Task<PetitionStatusDataResponse> GetPetitionStatus(PetitionStatusRequest petitionStatusRequest)
        {
            return _repo.GetPetitionStatus(petitionStatusRequest);
        }

        public Task<int> DeletePetitionStatus(PetitionStatusRequest petitionStatusRequest)
        {
            return _repo.DeletePetitionStatus(petitionStatusRequest);
        }

        public Task<int> UpdatePetitionStatus(PetitionStatusRequest petitionStatusRequest)
        {
            return _repo.UpdatePetitionStatus(petitionStatusRequest);
        }

    }

   

}
