using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Team;
namespace NTTLapso.Repository
{
    public class TeamRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

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
                SQLQueryGeneral += " AND Id={0}";
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
            string SQLQueryGeneral = String.Format("UPDATE team SET team='{1}', IdUserManager={2} WHERE Id={0}", request.Id, request.Team, request.IdManager);
            conn.Query(SQLQueryGeneral);

        }
        public async Task Delete(int Id)
        {
            string SQLQueryGeneral = String.Format("DELETE FROM team WHERE Id={0};", Id);
            conn.Query(SQLQueryGeneral);

        }
    }
}
