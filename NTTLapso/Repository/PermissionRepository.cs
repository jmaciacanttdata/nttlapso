using Dapper;
using MySqlConnector;
using NTTLapso.Models.Login;
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

        public async Task<List<LoginUserPermissionResponse>> ListUserPermission(int userId)
        {
            List < LoginUserPermissionResponse > response = new List < LoginUserPermissionResponse >();

            string idTeamQuery = "SELECT IdTeam FROM user_team WHERE IdUser = " + userId;
            List<int> idTeam = conn.Query<int>(idTeamQuery).ToList();//put into a list the teams which the user belongs

            foreach (int id in idTeam ) 
            {
                string SQLQuery = "SELECT T.Id AS TeamId, P.Value, P.Registration, P.Read, P.Edit, P.Delete FROM user U INNER JOIN " +
                    "user_team UT ON UT.IdUser = U.Id INNER JOIN " +
                    "team T ON T.Id = UT.IdTeam INNER JOIN " +
                    "user_team_rol_permission UTRP ON UTRP.IdUserTeam=UT.Id INNER JOIN " +
                    "rol_permission RP ON RP.Id=UTRP.IdRolPermission INNER JOIN " +
                    "rol R ON R.Id=RP.IdRol INNER JOIN " +
                    " permission P ON P.Id=RP.IdPermission WHERE " +
                    "U.Id = " + userId + " AND T.Id = " + id;

                string Query = String.Format(SQLQuery);
 
                List<LoginUserPermissionResponse> resp = (conn.Query<LoginUserPermissionResponse>(Query).ToList());
                foreach (var permissions in resp)
                {
                    response.Add(permissions);
                }
            }
            return response;
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
            string SQLSet = "";
            string SQLQueryPartial = "UPDATE permission SET";
            if (request.Value != null && request.Value != "")
                SQLSet += " Value='{1}'";

            if (request.Registration != null)
                SQLSet += ", `Registration`={2}";

            if (request.Read != null)
                SQLSet += ", `Read`={3}";

            if (request.Edit != null)
                SQLSet += ", `Edit`={4}";

            if (request.Delete != null)
                SQLSet += ", `Delete`={5}";

            if (SQLSet != "")
            {
                char[] stringArray = SQLSet.ToCharArray();
                if(stringArray[0] == ',' )
                    SQLSet = SQLSet.Substring(1, SQLSet.Length - 1);
            }

            SQLQueryPartial += SQLSet;
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
