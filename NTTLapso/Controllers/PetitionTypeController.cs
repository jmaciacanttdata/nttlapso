using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.General;
using NTTLapso.Models.Rol;
using NTTLapso.Service;
using Microsoft.AspNetCore.Authorization;
using NTTLapso.Models.PetitionType;
using System.Web.WebPages;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("PetitionType")]
    public class PetitionTypeController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PetitionTypeController> _logger;
        private PetitionTypeService _service = new PetitionTypeService();
        public PetitionTypeController(ILogger<PetitionTypeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        // Get petition type list.
        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<ListPetitionTypeResponse> List(PetitionTypeRequest? request)
        {
            ListPetitionTypeResponse response = new ListPetitionTypeResponse();
            List<PetitionTypeDataResponse> responseList = new List<PetitionTypeDataResponse>();
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

        // Create new petition type.
        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<PetitionTypeResponse> Create(string value, bool selectable)
        {
            PetitionTypeResponse response = new PetitionTypeResponse();

            try
            {
                if (!value.IsEmpty()) // Check if value is empty string.
                {
                    await _service.Create(value, selectable);
                    response.IsSuccess = true;
                }
                else
                {
                    Error _error = new Error("The value / selectable fields can't be empty or null", null);
                }
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;
            }

            return response;
        }

        // Edit a petition type.
        [HttpPost]
        [Route("Edit")]
        [Authorize]
        public async Task<PetitionTypeResponse> Edit(PetitionTypeRequest request)
        {
            PetitionTypeResponse response = new PetitionTypeResponse();

            try
            {
                if (!request.Value.IsEmpty() && request.Selectable.HasValue) // Check if value is empty string.
                {
                    await _service.Edit(request);
                    response.IsSuccess = true;
                }
                else
                {
                    Error _error = new Error("The value / selectable fields can't be empty or null", null);
                }
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        // Delete a petition type
        [HttpGet]
        [Route("Delete")]
        [Authorize]
        public async Task<PetitionTypeResponse> Delete(int Id)
        {
            PetitionTypeResponse response = new PetitionTypeResponse();

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
