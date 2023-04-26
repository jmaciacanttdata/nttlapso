using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.Rols;
using System;

namespace NTTLapso.Repository
{
    public class MastersRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public MastersRepository() { 
            conn = new MySqlConnection(connectionString);
        }

        // Register a new rol.
        internal async Task<RolRegisterResponse> RolRegister(RolRegisterRequest request)
        {
            RolRegisterResponse response = new RolRegisterResponse();
            string sqlSelect = "SELECT `value` FROM rol WHERE `value` = @Value;";
            string sqlInsert = "INSERT INTO rol (Value) Values (@Value);";

            try
            {
                conn.Open();

                var Value = await conn.QueryAsync(sqlSelect, new { Value = request.Value}); // Checks if the rol already exists

                if (Value.Count() == 0) // If doesn´t exist we insert rol
                {
                    var affectedRows = await conn.ExecuteAsync(sqlInsert, new { Value = request.Value });

                    if (affectedRows != 0)
                    {
                        response.IsRegistered = true;
                    }
                    else
                    {
                        response.IsRegistered = false;
                        response.ErrorType = "Internal Server Error";
                        response.ErrorMessage = "The request couldn't be inserted in DB";
                    }
                }
                else
                {
                    response.IsRegistered = false;
                    response.ErrorType = "Already Exists";
                    response.ErrorMessage = $"The {request.Value} rol already exists";
                }
            }
            catch (Exception ex)
            {
                response.IsRegistered = false;
                response.ErrorType = "Internal Server Error";
                response.ErrorMessage = ex.Message;
            }
            finally 
            { 
                conn.Close(); 
            }

            return response;
        }

        // Get a list of rols.
        internal async Task<GetRolsListResponse> GetRolsList()
        {
            GetRolsListResponse response = new GetRolsListResponse();
            string sqlSelect = "SELECT * FROM rol";

            try
            {
                conn.Open();

                var Rols = await conn.QueryAsync<IdValue>(sqlSelect);

                if (Rols.Any())
                {
                    response.RolsList = Rols.ToList();
                }
                else
                {
                    response.ErrorType = "Data Base Error";
                    response.ErrorMessage = "Couldn´t get data to make list from data base";
                }
            }
            catch (Exception ex)
            {
                response.ErrorType = "Internal Server Error";
                response.ErrorMessage = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return response;
        }

        // Get a list of rols.
        internal async Task<UpdateRolResponse> UpdateRol(UpdateRolRequest request)
        {
            UpdateRolResponse response = new UpdateRolResponse();
            string sqlSelect = "SELECT `value` FROM rol WHERE `value` = @Value;";
            string sqlUpdate = "UPDATE rol SET Value = @Value WHERE Id = @Id";

            try
            {
                conn.Open();
                var Value = await conn.QueryAsync(sqlSelect, new { Value = request.Value }); // Checks if the rol already exists

                if (Value.Count() == 0) // If doesn´t exist we insert rol
                {
                    var Rols = await conn.ExecuteAsync(sqlUpdate, request);

                    if (Rols != 0)
                    {
                        response.IsUpdated = true;
                    }
                    else
                    {
                        response.IsUpdated = false;
                        response.ErrorType = "Data Base Error";
                        response.ErrorMessage = "No rols updated in Data Base";
                    }
                }
                else
                {
                    response.IsUpdated = false;
                    response.ErrorType = "Already Exists";
                    response.ErrorMessage = $"Can´t update rol because the {request.Value} rol already exists";
                }
            }
            catch (Exception ex)
            {
                response.ErrorType = "Internal Server Error";
                response.ErrorMessage = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return response;
        }

        // Delete a rol.
        internal async Task<UpdateRolResponse> DeleteRol(int rolId)
        {
            UpdateRolResponse response = new UpdateRolResponse();
            string sqlSelect = $"SELECT `value` FROM rol WHERE `id` = {rolId};";
            string sqlDelete = $"DELETE FROM rol WHERE Id = {rolId};";

            try
            {
                conn.Open();
                var Value = await conn.QueryAsync(sqlSelect); // Checks if the rol exists

                if (Value.Count() != 0) // If exists we delete rol
                {
                    var Rols = await conn.ExecuteAsync(sqlDelete);

                    if (Rols != 0)
                    {
                        response.IsUpdated = true;
                    }
                    else
                    {
                        response.IsUpdated = false;
                        response.ErrorType = "Data Base Error";
                        response.ErrorMessage = "Rol couldn´t be deleted";
                    }
                }
                else
                {
                    response.IsUpdated = false;
                    response.ErrorType = "Id not found";
                    response.ErrorMessage = $"Can´t delete rol because the id: {rolId} doesn´t exist.";
                }
            }
            catch (Exception ex)
            {
                response.ErrorType = "Internal Server Error";
                response.ErrorMessage = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            return response;
        }
    }
}
