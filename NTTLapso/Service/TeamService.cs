using NTTLapso.Models.General;
using NTTLapso.Models.Team;
using NTTLapso.Repository;


namespace NTTLapso.Service
{
    public class TeamService
    {
        private TeamRepository _repo;
        private IConfiguration _configuration;
        public TeamService(IConfiguration config) 
        {
            _configuration = config;
            _repo = new TeamRepository(_configuration);
        }
        public async Task<List<TeamData>> List() => await _repo.List();
        public async Task<bool> IsTeamLeader(int idManager) => await _repo.IsTeamLeader(idManager);
        public async Task<List<TeamData>> ListTeamsByLeaderId(int idManager) => await _repo.ListTeamsByLeaderId(idManager);
        public async Task<List<TeamData>> GetTeamGridData() => await _repo.GetTeamGridData();

        public async Task Create(TeamRequest request)
        {
            await _repo.Create(request);
        }

        public async Task Edit(TeamRequest request)
        {
            await _repo.Edit(request);
        }

        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }

        public async Task<List<TeamManagerDataResponse>> GetTeamsManagerList(int IdTeam, int IdUser)
        {
            return await _repo.GetTeamsManagerList(IdTeam, IdUser);
        }

    }
}
