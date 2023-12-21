using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Team;
using NTTLapso.Support_methods;

namespace NTTLapso.Repository
{
    public class TeamRepository
    {
        private static string connectionString;
        private MySqlConnection conn;
        private SupportMethods check = new SupportMethods();
        private IConfiguration _config;
        public TeamRepository(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetValue<string>("ConnectionStrings:Develop");
            conn = new MySqlConnection(connectionString);
            _config = config;
        }

        public async Task<List<TeamData>> List()
        {
            var result = conn.Query<TeamData>(String.Format("SELECT * FROM team")).ToList();
            return result.Select(team => new TeamData { Id = team.Id, Team = team.Team}).ToList();
        }

        public async Task<bool> IsTeamLeader(int idManager)
        {
            return conn.Query(String.Format("SELECT Id_Leader from team_supervised_by where Id_Leader = {0}", idManager)).Count() > 0;
        }

        public async Task<List<TeamData>> ListTeamsByLeaderId(int idManager)
        {
            List<TeamData> response = new List<TeamData>();

            string query = String.Format("SELECT t.Id, Team FROM team t INNER JOIN team_supervised_by tsb ON tsb.Id_Team = t.Id where Id_Leader = {0}", idManager);
            response = conn.Query<TeamData>(query).ToList();

            return response;
        }

        public async Task<List<TeamData>> GetTeamGridData()
        {
            List<TeamData> response = new List<TeamData>();

            string query = "SELECT t.Id, tsb.Id_Leader, t.Team, name AS 'Username'FROM team_supervised_by tsb INNER JOIN user u ON u.Id = tsb.Id_Leader INNER JOIN team t ON t.Id = tsb.Id_Team ORDER BY t.Team ASC";
            string sqlQuery = String.Format(query);
            var queryResponse = conn.Query(sqlQuery);

            foreach(var row in  queryResponse)
            {
                TeamData teamData = new TeamData
                {
                    Id = row.Id,
                    Team = row.Team,
                    IdManager = row.Id_Leader,
                    Manager = row.Username
                };
                response.Add(teamData);
            }


            return response;
        }

        public async Task Create(TeamRequest request)
        {
            string SQLQueryGeneral = String.Format("INSERT INTO team(team, IdUserManager) VALUES('{0}', {1})", request.Team, request.IdManager);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Edit(TeamRequest request)
        {
            //string SQLQueryGeneral = String.Format("UPDATE team SET team='{1}', IdUserManager={2} WHERE Id={0}", request.Id, request.Team, request.IdManager);
            //conn.Query(SQLQueryGeneral);
            string SQLSet = "";
            string SQLQueryPartial = "UPDATE team SET";
            if (request.Team != null && request.Team != "")
                SQLSet += " Team='{1}'";

            if (request.IdManager != null && request.IdManager != 0)
                SQLSet += ", `IdUserManager`= {2}";

            if (SQLSet != "")
            {
                SQLSet = check.CheckCommas(SQLSet);
            }

            SQLQueryPartial += SQLSet;
            SQLQueryPartial += " WHERE Id={0};";

            string SQLQueryGeneral = String.Format(SQLQueryPartial, request.Id, request.Team, request.IdManager);
            conn.Query(SQLQueryGeneral);
        }

        public async Task Delete(int Id)
        {
            string SQLQueryGeneral = String.Format("DELETE FROM team WHERE Id={0};", Id);
            conn.Query(SQLQueryGeneral);

        }

        public async Task<List<TeamManagerDataResponse>> GetTeamsManagerList(int IdTeam, int IdUser)
        {
            string SQLQueryGeneral = "SELECT DISTINCT t.IdUserManager AS 'Id', user.Name AS 'Name', user.Email AS 'Email' " +
                "FROM team t JOIN `user` ON user.Id = t.IdUserManager JOIN user_team ut ON t.Id = ut.IdTeam WHERE 1=1";
            if (IdTeam  > 0)
            {
                SQLQueryGeneral += " AND t.Id = {0}";
            }
            if (IdUser > 0)
            {
                SQLQueryGeneral += " AND ut.IdUser = {1}";
            }

            string SQLQuery = string.Format(SQLQueryGeneral, IdTeam, IdUser);
            
            List<TeamManagerDataResponse> response = (conn.Query<TeamManagerDataResponse>(SQLQuery)).ToList();

            return response;
        }

    }
}
