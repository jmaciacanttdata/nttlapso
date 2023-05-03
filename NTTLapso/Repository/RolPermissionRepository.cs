using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.RolPermission;

namespace NTTLapso.Repository
{
    public class RolPermissionRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public RolPermissionRepository()
        {
            conn = new MySqlConnection(connectionString);
        }

        // Get a list of rols with it's permissions.
        public async Task<List<RolPermissionDataResponse>> List(int? idRol)
        {
            string SQLQueryGeneral = "SELECT DISTINCT rol.id AS IdRol, rol.value FROM rol_permission INNER JOIN rol ON rol_permission.idrol = rol.id WHERE 1=1";

            if (idRol != null && idRol > 0)
            {
                SQLQueryGeneral += " AND IdRol={0}";
            }

            string SQLQuery = String.Format(SQLQueryGeneral, idRol);


            List<RolPermissionDataResponse> response = (await conn.QueryAsync<RolPermissionDataResponse>(SQLQuery)).ToList(); 

            if (response.Any())
            {
                foreach (var rol in response)
                {
                    string sqlPermissions = "SELECT permission.id, permission.value, permission.registration, permission.read, permission.edit, permission.delete FROM rol_permission INNER JOIN permission ON rol_permission.idpermission = permission.id WHERE IdRol = @Idrol;";
                    rol.PermissionList = (await conn.QueryAsync<PermissionDataResponse>(sqlPermissions, new { IdRol = rol.IdRol })).ToList();
                }
            }
            else
            {
                throw new Exception(message: "There is no data in the database or there is none matching the applied filter.");
            }

            return response;
        }

        // Insert a new rol with permissions.
        public async Task Create(RolPermissionRequest request)
        {
            string SQLQueryGeneral = "SELECT `IdRol` FROM rol_permission WHERE `IdRol` = {0};";

            string SQLQuery = String.Format(SQLQueryGeneral, request.IdRol);

            var response = await conn.QueryAsync(SQLQuery);

            if (response.Count() == 0)
            {
                foreach (var permission in request.PermissionList)
                {
                    string sqlInsert = "INSERT INTO rol_permission (IdRol, IdPermission) VALUES ({0}, {1});";
                    string SQLInsertQuery = String.Format(sqlInsert, request.IdRol, permission);
                    await conn.ExecuteAsync(SQLInsertQuery);
                }
            }
            else
            {
                throw new Exception(message: $"There is already data in the database matching the id {request.IdRol}");
            }
        }

        // Update permissions from a rol.
        public async Task Edit(RolPermissionRequest request)
        {
            string SQLQueryGeneral = "SELECT `IdRol` FROM rol_permission WHERE `IdRol` = {0};";

            string SQLQuery = String.Format(SQLQueryGeneral, request.IdRol);

            var response = await conn.QueryAsync(SQLQuery); // Check if Rol exits

            if (response.Count() != 0) // If exists we delete data and proceed to insert new data.
            {
                string sqlDelete = "DELETE FROM rol_permission WHERE `idrol` = @IdRol;";
                var petitionType = await conn.ExecuteAsync(sqlDelete, new { IdRol = request.IdRol });

                if (petitionType != 0) // If rol is deleted we insert new rol with new permissions.
                {
                    foreach (var permission in request.PermissionList)
                    {
                        string sqlInsert = "INSERT INTO rol_permission (IdRol, IdPermission) VALUES ({0}, {1});";
                        string SQLInsertQuery = String.Format(sqlInsert, request.IdRol, permission);
                        await conn.ExecuteAsync(SQLInsertQuery);
                    }
                }
            }
            else
            {
                throw new Exception(message: $"There is no data in the database matching the id {request.IdRol}");
            }
        }

        // Delete rol and it's permissions.
        public async Task Delete(int idRol)
        {
            string SQLQueryGeneral = "SELECT `IdRol` FROM rol_permission WHERE `IdRol` = {0};";

            string SQLQuery = String.Format(SQLQueryGeneral, idRol);

            var response = await conn.QueryAsync(SQLQuery); // Check if Rol exits

            if (response.Count() != 0) // If exists we delete data.
            {
                string sqlDelete = "DELETE FROM rol_permission WHERE `idrol` = @IdRol;";
                var petitionType = await conn.ExecuteAsync(sqlDelete, new { IdRol = idRol });
            }
            else
            {
                throw new Exception(message: $"There is no data in the database matching the id {idRol}");
            }
        }
    }
}
