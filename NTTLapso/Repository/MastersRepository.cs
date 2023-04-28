using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.RolPermission;
using System.Security;

namespace NTTLapso.Repository
{
    public class MastersRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public MastersRepository() { 
            conn = new MySqlConnection(connectionString);
        }

        // Register a new rol with it's permissions.
        internal async Task<ErrorResponse> SetRolPermission(SetRolPermissionRequest request)
        {
            ErrorResponse response = new ErrorResponse();
            var sqlRol = "SELECT `IdRol` FROM rol_permission WHERE `IdRol` = @IdRol;";
            string sqlInsert = "INSERT INTO rol_permission (IdRol, IdPermission) VALUES (@IdRol, @IdPermission);";

            try
            {
                using (conn)
                {
                    var Value = await conn.QueryAsync(sqlRol,
                        new { IdRol = request.IdRol }); // Checks if the rol already exists

                    if (Value.Count() == 0) // If doesn´t exist we insert rol and permissions.
                    {
                        foreach (var permission in request.PermissionList)
                        {
                            var affectedRows = await conn.ExecuteAsync(sqlInsert, new { IdRol = request.IdRol, IdPermission = permission });
                        }

                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.ErrorType = "Already Exists";
                        response.ErrorMessage = $"The rol with Id: {request.IdRol} already exists";
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorType = "Internal Server Error";
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        // Get a list of rols with it's permissions.
        internal async Task<GetRolPermissionResponse> GetRolsPermissionList()
        {
            GetRolPermissionResponse response = new GetRolPermissionResponse();
            string sqlRolIds = "SELECT DISTINCT rol.id AS IdRol, rol.value FROM rol_permission INNER JOIN rol ON rol_permission.idrol = rol.id;";
            string sqlPermissions = "SELECT permission.id, permission.value, permission.registration, permission.read, permission.edit, permission.delete FROM rol_permission INNER JOIN permission ON rol_permission.idpermission = permission.id WHERE IdRol = @Idrol;";

            try
            {
                using (conn)
                {
                    List<RolPermissionDataResponse> rols= (await conn.QueryAsync<RolPermissionDataResponse>(sqlRolIds)).ToList();

                    if (rols.Any())
                    {
                        foreach (var rol in rols)
                        {
                            rol.PermissionDataList = (await conn.QueryAsync<PermissionDataResponse>(sqlPermissions, new { IdRol = rol.IdRol })).ToList();
                        }

                        response.Error.IsSuccess = true;
                        response.Data = rols;
                    }
                    else
                    {
                        response.Error.IsSuccess = false;
                        response.Error.ErrorType = "Data Base Error";
                        response.Error.ErrorMessage = "Couldn´t get data from data base, no rols exist";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Error.IsSuccess = false;
                response.Error.ErrorType = "Internal Server Error";
                response.Error.ErrorMessage = ex.Message;
            }

            return response;
        }

        // Update rol permissions.
        internal async Task<ErrorResponse> UpdateRolPermission(SetRolPermissionRequest request)
        {
            ErrorResponse response = new ErrorResponse();
            string sqlSelect = "SELECT `IdRol` FROM rol_permission WHERE `idrol` = @IdRol;";
            string sqlDelete = "DELETE FROM rol_permission WHERE `idrol` = @IdRol;";
            string sqlInsert = "INSERT INTO rol_permission (IdRol, IdPermission) VALUES (@IdRol, @IdPermission);";

            try
            {
                using (conn)
                {
                    var Value = await conn.QueryAsync(sqlSelect, new { IdRol = request.IdRol }); // Checks if rol exists

                    if (Value.Count() != 0) // If exists we delete rol and it's permission/s.
                    {
                        var petitionType = await conn.ExecuteAsync(sqlDelete, new { IdRol = request.IdRol });

                        if (petitionType != 0) // If rol is deleted we insert new rol with new permissions.
                        {
                            foreach (var permission in request.PermissionList)
                            {
                                var affectedRows = await conn.ExecuteAsync(sqlInsert, new { IdRol = request.IdRol, IdPermission = permission });
                            }

                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.ErrorType = "Data Base Error";
                            response.ErrorMessage = "No rol permissions updates in Data Base";
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.ErrorType = "Doesn´t Exists";
                        response.ErrorMessage = $"Can´t update rol permissions because the id: {request.IdRol} doesn´t exist.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorType = "Internal Server Error";
                response.ErrorMessage = ex.Message;
            }

            return response;
        }

        // Delete rol permissions.
        internal async Task<ErrorResponse> DeleteRolPermissions(int rolId)
        {
            ErrorResponse response = new ErrorResponse();
            string sqlSelect = "SELECT `IdRol` FROM rol_permission WHERE `idrol` = @rolId;";
            string sqlDelete = "DELETE FROM rol_permission WHERE `idrol` = @rolId;";

            try
            {
                using (conn)
                {
                    var Value = await conn.QueryAsync(sqlSelect, new { rolId = rolId }); // Checks if rol exists

                    if (Value.Count() != 0) // If exists we delete rol and it's permission/s.
                    {
                        var petitionType = await conn.ExecuteAsync(sqlDelete, new { rolId = rolId });

                        if (petitionType != 0)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.ErrorType = "Data Base Error";
                            response.ErrorMessage = "No rol permissions deleted in Data Base";
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.ErrorType = "Doesn´t Exists";
                        response.ErrorMessage = $"Can´t delete rol permissions because the id: {rolId} doesn´t exist.";
                    }
                }
            }
            catch (Exception ex)
            {
                response.ErrorType = "Internal Server Error";
                response.ErrorMessage = ex.Message;
            }

            return response;
        }
    }
}
