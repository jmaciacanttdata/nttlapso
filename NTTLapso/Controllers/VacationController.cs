using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.General;
using NTTLapso.Models.Vacations;
using NTTLapso.Repository;
using NTTLapso.Service;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("Vacation")]
    public class VacationController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<VacationController> _logger;
        private VacationService _service = new VacationService();
        public VacationRepository _repo = new VacationRepository();
        public VacationController(ILogger<VacationController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;

        }

        [HttpPost]
        [Route("Create")]
        [AllowAnonymous]
        public async Task<VacationResponse> Create(CreateVacationRequest request)
        {
            VacationResponse response = new VacationResponse();
            try
            {
                if (request.IdUserPetition != 0)
                {
                    if (await _repo.CheckViability(request.IdUserPetition, request.Day))
                    {
                        await _service.Create(request);
                        response.IsSuccess = true;
                    }
                    else
                    {
                        Error _error = new Error(" The selected date isn´t available in your team ", null);
                        response.IsSuccess = false;
                        response.Error = _error;
                    }
                    //Llamar a sendAltaNotification()
                }
                else
                {
                    response.IsSuccess = false;
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
        [HttpPost]
        [Route("Edit")]
        [AllowAnonymous]
        public async Task<VacationResponse> Edit(EditVacationRequest request)
        {
            VacationResponse response = new VacationResponse();
            try
            {
                if (request.IdUserPetition != 0)
                {
                    if (await _repo.CheckViability(request.IdUserPetition, request.PetitionDate))
                    {
                        await _service.Edit(request);
                        response.IsSuccess = true;
                    }
                    else
                    {
                        Error _error = new Error(" The selected date isn´t available in your team ", null);
                        response.IsSuccess = false;
                        response.Error = _error;
                    }
                    //Llamar a sendAltaNotification()
                }
                else
                {
                    response.IsSuccess = false;
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
    }
}
