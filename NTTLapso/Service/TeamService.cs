using NTTLapso.Models.General;
using NTTLapso.Models.Team;
using NTTLapso.Repository;


namespace NTTLapso.Service
{
    public class TeamService
    {
        private TeamRepository _repo = new TeamRepository();
        public TeamService() { }
        public async Task<List<TeamData>> List(TeamRequest request)
        {
            return await _repo.List(request);
        }

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
