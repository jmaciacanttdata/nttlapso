using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;

namespace NTTLapso.Repository
{
    public class UserScheduleRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public UserScheduleRepository()
        {
            conn = new MySqlConnection(connectionString);
        }

        public async Task<List<IdValue>> List(IdValue? request)
        {
            List<IdValue> response = new List<IdValue>();

            string SQLQueryGeneral = "SELECT Id, Value FROM user_schedule WHERE 1=1";
            if (request != null && request.Id > 0)
                SQLQueryGeneral += " AND Id={0}";
            if (request != null && request.Value != null && request.Value != "")
                SQLQueryGeneral += " AND Value LIKE '%{1}%'";

            string SQLQuery = String.Format(SQLQueryGeneral, request.Id, request.Value);

            response = conn.Query<IdValue>(SQLQuery).ToList();

            if (response.Count == 0)
            {
                throw new Exception(message: "No results found for the request");
            }

            return response;
        }

        public async Task Create(string value)
        {
                string SQLQueryGeneral = String.Format("INSERT INTO user_schedule(Value) VALUES('{0}')", value);
                conn.Query(SQLQueryGeneral);

        }

        public async Task Edit(IdValue request)
        {
            string SQLQueryGeneral = String.Format("UPDATE user_schedule SET Value='{1}' WHERE Id={0}", request.Id, request.Value);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Delete(int Id)
        {
            string SQLQueryGeneral = String.Format("DELETE FROM user_schedule WHERE Id={0};", Id);
            conn.Query(SQLQueryGeneral);

        }
    }
}
