using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Team;
using NTTLapso.Support_methods;

namespace NTTLapso.Repository
{
    public class TeamRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;
        private SupportMethods check = new SupportMethods();
        public TeamRepository()
        {
            conn = new MySqlConnection(connectionString);
        }

        public async Task<List<TeamData>> List(TeamRequest? request)
        {
            List<TeamData> response = new List<TeamData>();

            string SQLQueryGeneral = "SELECT team.Id, Team, CONCAT(user.Name,' ',user.Surnames) AS 'Manager' FROM team INNER JOIN user ON team.IdUserManager = user.Id WHERE 1=1";
            if (request != null && request.Id > 0)
            {
                SQLQueryGeneral += " AND team.Id={0}";
            }
            if(request != null && request.Team != null && request.Team != "")
            {
                SQLQueryGeneral += " AND Team LIKE '%{1}%'";
            }
            if (request != null && request.IdManager > 0)
            {
                SQLQueryGeneral += " AND IdUserManager={2}";
            }
            string SQLQuery = String.Format(SQLQueryGeneral, request.Id, request.Team, request.IdManager);

            response = conn.Query<TeamData>(SQLQuery).ToList();
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
            string SQLQueryGeneral = "SELECT DISTINCT t.IdUserManager AS 'Id', CONCAT(user.Name, ' ', user.surnames) AS 'Name', user.Email AS 'Email' " +
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
