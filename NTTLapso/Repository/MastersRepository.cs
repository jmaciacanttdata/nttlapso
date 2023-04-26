using Dapper;
using MySqlConnector;
using NTTLapso.Models.Login;
using NTTLapso.Models.Permissions;

namespace NTTLapso.Repository
{
    public class MastersRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public MastersRepository() { 
            conn = new MySqlConnection(connectionString);
        }
        public async Task<int> PermissionRegister(PermissionRequest permissionRequest)
        {
            int response = 0;
            string SQLQuery = "INSERT INTO permission (`Value`, `Registration`, `Read`, `Edit`, `Delete`) VALUES ('" + permissionRequest.Value + "', '"+ permissionRequest.Registration + "', " + permissionRequest.Read + ", "+permissionRequest.Edit + ", "+permissionRequest.Delete +");";
            response = conn.Execute(SQLQuery);
            return response;
        }

        public async Task<PermissionDataResponse> GetPermission(PermissionRequest permissionRequest)
        {
            PermissionDataResponse response = new PermissionDataResponse();
            string SQLQuery = "SELECT Id, `Value`, `Registration`, `Read`, `Edit`, `Delete` FROM permission WHERE Id = " + permissionRequest.Id;
            response =  conn.Query<PermissionDataResponse>(SQLQuery).FirstOrDefault();
            return response;
        }

        public async Task<int> DeletePermission(PermissionRequest permissionRequest)
        {
            int response = 0;
            string SQLQuery = "DELETE FROM permission WHERE Id = " + permissionRequest.Id;
            response = conn.Execute(SQLQuery);
            return response;
        }

        public async Task<int> UpdatePermission(PermissionRequest permissionRequest)
        {
            int response = 0;
            string SQLQuery = "UPDATE permission SET `Value` = '"+permissionRequest.Value + "' , `Registration` = " + permissionRequest.Registration + ", `Read` = " + permissionRequest.Read + ", `Edit` = " + permissionRequest.Edit + ", `Delete` = " + permissionRequest.Delete+ " WHERE Id = " + permissionRequest.Id;
            response = conn.Execute(SQLQuery);
            return response;
        }
    }
}
