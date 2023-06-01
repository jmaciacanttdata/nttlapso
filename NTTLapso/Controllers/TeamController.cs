using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.General;
using NTTLapso.Models.Team;
using NTTLapso.Service;


namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("NTTLapso/Team")]
    public class TeamController
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TeamController> _logger;
        private TeamService _service;
        public TeamController(ILogger<TeamController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new TeamService(_config);
        }


        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<ListTeamResponse> List(TeamRequest? request)
        {
            ListTeamResponse response = new ListTeamResponse();
            List<TeamData> responseList = new List<TeamData>();
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
        [Authorize]
        public async Task<TeamResponse> Create(TeamRequest request)
        {
            TeamResponse response = new TeamResponse();

            try
            {
                await _service.Create(request);
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

        [HttpPost]
        [Route("Edit")]
        [Authorize]
        public async Task<TeamResponse> Edit(TeamRequest request)
        {
            TeamResponse response = new TeamResponse();

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
        [Authorize]
        public async Task<TeamResponse> Delete(int Id)
        {
            TeamResponse response = new TeamResponse();

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
