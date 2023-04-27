using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionType;
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

        // Register a new petition type.
        internal async Task<ErrorResponse> PetitionTypeRegister(PetitionTypeRegisterRequest request)
        {
            return await _repo.PetitionTypeRegister(request);
        }

        // Get a petition type.
        internal async Task<GetPetitionTypeResponse> GetPetitionType(int petitionTypeId)
        {
            return await _repo.GetPetitionType(petitionTypeId);
        }

        // Get a list of petition types.
        public async Task<GetPetitionTypeListResponse> GetPetitionTypeList()
        {
            return await _repo.GetPetitionTypeList();
        }

        // Update a petition type.
        internal async Task<ErrorResponse> UpdatePetitionType(UpdatePetitionTypeRequest request)
        {
            return await _repo.UpdatePetitionType(request);
        }

        // Delete a petition type.
        internal async Task<ErrorResponse> DeletePetitionType(int petitionTypeId)
        {
            return await _repo.DeletePetitionType(petitionTypeId);
        }
    }
}
