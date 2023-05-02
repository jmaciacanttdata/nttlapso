using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;

namespace NTTLapso.Repository
{
    public class PetitionStateRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public PetitionStateRepository() { 
            conn = new MySqlConnection(connectionString);
        }

        public async Task<List<IdValue>> List(IdValue? request) {
            List<IdValue> response = new List<IdValue>();

            string SQLQueryGeneral = "SELECT Id, Value FROM petition_state WHERE 1=1";
            if (request != null && request.Id > 0)
                SQLQueryGeneral += " AND Id={0}";
            if (request != null && request.Value != null && request.Value != "")
                SQLQueryGeneral += " AND Value LIKE '%{1}%'";

            string SQLQuery = String.Format(SQLQueryGeneral, request.Id, request.Value);

            response = conn.Query<IdValue>(SQLQuery).ToList();
            return response;
        }

        public async Task Create(CreatePetitionStatusRequest request)
        {
            string SQLQueryHeaders = "INSERT INTO petition_state(Value";
            string SQLQueryValues = "VALUES('{0}'";
            
            if( request.IdTextNotification!=null && request.IdTextNotification>0)
            {
                SQLQueryHeaders += ", IdTextNotification) ";
                SQLQueryValues += ", {1});";
            }
            else
            {
                SQLQueryHeaders += ") ";
                SQLQueryValues += ");";
            }

            string SQLQueryGeneral = String.Format((SQLQueryHeaders + SQLQueryValues), request.Value, request.IdTextNotification);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Edit(EditPetitionStatusRequest request)
        {
            string SQLQueryPartial = "UPDATE petition_state SET Value='{1}'";
            if (request.IdTextNotification != null && request.IdTextNotification > 0)
                SQLQueryPartial += ", IdTextNotification={2}";

            SQLQueryPartial += " WHERE Id={0};";

            string SQLQueryGeneral = String.Format(SQLQueryPartial, request.Id, request.Value, request.IdTextNotification);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Delete(int Id)
        {
            string SQLQueryGeneral = String.Format("DELETE FROM petition_state WHERE Id={0};", Id);
            conn.Query(SQLQueryGeneral);

        }
    }
}
