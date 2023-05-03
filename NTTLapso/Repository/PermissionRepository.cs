using Dapper;
using MySqlConnector;
using NTTLapso.Models.Permissions;

namespace NTTLapso.Repository
{
    public class PermissionRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public PermissionRepository()
        {
            conn = new MySqlConnection(connectionString);
        }

        public async Task<List<PermissionDataResponse>> List(PermissionDataResponse? request)
        {
            List<PermissionDataResponse> response = new List<PermissionDataResponse>();

            string SQLQueryGeneral = "SELECT Id, `Value`, `Registration`, `Read`, `Edit`, `Delete` FROM permission WHERE 1=1";
            if (request != null && request.Id > 0)
                SQLQueryGeneral += " AND Id={0}";
            if (request != null && request.Value != null && request.Value != "")
                SQLQueryGeneral += " AND Value LIKE '%{1}%'";

            string SQLQuery = String.Format(SQLQueryGeneral, request.Id, request.Value);

            response = conn.Query<PermissionDataResponse>(SQLQuery).ToList();

            if(response.Count == 0)
            {
                throw new Exception(message: "No se encontraron resultados para la búsqueda");
            }
            return response;
        }

        public async Task Create(CreatePermissionRequest request)
        {
            string SQLQueryHeaders = "INSERT INTO permission(`Value`, `Registration`, `Read`, `Edit`, `Delete`) ";
            string SQLQueryValues = "VALUES('{0}', {1}, {2}, {3}, {4})";

            string SQLQueryGeneral = String.Format((SQLQueryHeaders + SQLQueryValues), request.Value, request.Registration, request.Read, request.Edit, request.Delete);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Edit(EditPermissionRequest request)
        {
            string SQLQueryPartial = "UPDATE permission SET";
            if (request.Value != null && request.Value != "")
                SQLQueryPartial += " Value='{1}'";

            if (request.Registration != null)
                SQLQueryPartial += ", `Registration`={2}";

            if (request.Read != null)
                SQLQueryPartial += ", `Read`={3}";

            if (request.Edit != null)
                SQLQueryPartial += ", `Edit`={4}";

            if (request.Delete != null)
                SQLQueryPartial += ", `Delete`={5}";

            SQLQueryPartial += " WHERE Id={0};";

            string SQLQueryGeneral = String.Format(SQLQueryPartial, request.Id, request.Value, request.Registration, request.Read, request.Edit, request.Delete);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Delete(int Id)
        {
            string SQLQueryGeneral = String.Format("DELETE FROM permission WHERE Id={0};", Id);
            conn.Query(SQLQueryGeneral);

        }
    }
}
