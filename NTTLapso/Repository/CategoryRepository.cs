using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;

namespace NTTLapso.Repository
{
    public class CategoryRepository
    {
        private static string connectionString;
        private MySqlConnection conn;
        private IConfiguration _config;
        public CategoryRepository(IConfiguration config) {
            _config = config;
            connectionString = _config.GetValue<string>("ConnectionStrings:Develop");
            conn = new MySqlConnection(connectionString);
        }

        public async Task<List<IdValue>> List(IdValue? request) {
            List<IdValue> response = new List<IdValue>();

            string SQLQueryGeneral = "SELECT Id, Value FROM category WHERE 1=1";
            if (request != null && request.Id > 0)
                SQLQueryGeneral += " AND Id={0}";
            if (request != null && request.Value != null && request.Value != "")
                SQLQueryGeneral += " AND Value LIKE '%{1}%'";

            string SQLQuery = String.Format(SQLQueryGeneral, request.Id, request.Value);

            response = conn.Query<IdValue>(SQLQuery).ToList();
            return response;
        }

        public async Task Create(string value)
        {
            string SQLQueryGeneral = String.Format("INSERT INTO category(Value) VALUES('{0}')", value);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Edit(IdValue request)
        {
            string SQLQueryGeneral = String.Format("UPDATE category SET Value='{1}' WHERE Id={0}", request.Id, request.Value);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Delete(int Id)
        {
            string SQLQueryGeneral = String.Format("DELETE FROM category WHERE Id={0};", Id);
            conn.Query(SQLQueryGeneral);

        }
    }
}
