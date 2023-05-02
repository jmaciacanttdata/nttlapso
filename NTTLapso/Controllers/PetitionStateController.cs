using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;
using NTTLapso.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("PetitionState")]
    public class PetitionStateController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PetitionStateController> _logger;
        private PetitionStateService _service = new PetitionStateService();
        public PetitionStateController(ILogger<PetitionStateController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }


        [HttpPost]
        [Route("List")]
        [AllowAnonymous]
        public async Task<ListPetitionStatusResponse> ListPetitionStatus(IdValue? request) {
            ListPetitionStatusResponse response = new ListPetitionStatusResponse();
            List<IdValue> responseList = new List<IdValue>();
            try
            {
                responseList = await _service.List(request);
                response.IsSuccess = true;
                response.Data = responseList;
                response.Error = null;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;
            }

            return response;
        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public async Task<PeticionStatusResponse> CreatePetitionStatus(CreatePetitionStatusRequest request)
        {
            PeticionStatusResponse response = new PeticionStatusResponse();

            try
            {
                await _service.Create(request);
                response.IsSuccess = true;
            }catch(Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        [HttpPost]
        [Route("Edit")]
        [AllowAnonymous]
        public async Task<PeticionStatusResponse> EditPetitionStatus(EditPetitionStatusRequest request)
        {
            PeticionStatusResponse response = new PeticionStatusResponse();

            try
            {
                await _service.Edit(request);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        [HttpGet]
        [Route("Delete")]
        [AllowAnonymous]
        public async Task<PeticionStatusResponse> DeletePetitionStatus(int Id)
        {
            PeticionStatusResponse response = new PeticionStatusResponse();

            try
            {
                await _service.Delete(Id);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }
    }
}
