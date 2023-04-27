using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionType;

namespace NTTLapso.Repository
{
    public class MastersRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public MastersRepository()
        {
            conn = new MySqlConnection(connectionString);
        }

        // Register a new petition type.
        internal async Task<ErrorResponse> PetitionTypeRegister(PetitionTypeRegisterRequest request)
        {
            ErrorResponse response = new ErrorResponse();
            string sqlSelect = "SELECT `value` FROM petition_type WHERE LOWER(`value`) = LOWER(@Value);";
            string sqlInsert = "INSERT INTO petition_type (Value, Selectable) VALUES (@Value, @Selectable);";

            try
            {
                using (conn)
                {
                    var Value = await conn.QueryAsync(sqlSelect,
                        new { Value = request.Value }); // Checks if the petition type already exists

                    if (Value.Count() == 0) // If doesn´t exist we insert petition type
                    {
                        var affectedRows = await conn.ExecuteAsync(sqlInsert, new { Value = request.Value, Selectable = request.Selectable });

                        if (affectedRows != 0)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.ErrorType = "Internal Server Error";
                            response.ErrorMessage = "The request couldn't be inserted in DB";
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.ErrorType = "Already Exists";
                        response.ErrorMessage = $"The {request.Value} petition type already exists";
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

        // Get a petition type.
        internal async Task<GetPetitionTypeResponse> GetPetitionType(int petitionTypeId)
        {

            GetPetitionTypeResponse response = new GetPetitionTypeResponse();
            string sqlSelect = "SELECT `Value`, `Selectable` FROM petition_type WHERE `Id` = @Id";

            try
            {
                using (conn)
                {
                    var petitionType = (await conn.QueryAsync<PetitionTypeDataResponse>(sqlSelect, new { Id = petitionTypeId })).FirstOrDefault();

                    if (petitionType != null)
                    {
                        response.Error.IsSuccess = true;
                        response.Data.Id = petitionTypeId;
                        response.Data.Value = petitionType.Value;
                        response.Data.Selectable = petitionType.Selectable;
                    }
                    else
                    {
                        response.Error.IsSuccess = false;
                        response.Error.ErrorType = "Data Base Error";
                        response.Error.ErrorMessage = "Couldn´t get data from data base, the Id doesn´t exist";
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

        // Get a list of petition types.
        public async Task<GetPetitionTypeListResponse> GetPetitionTypeList()
        {
            GetPetitionTypeListResponse response = new GetPetitionTypeListResponse();
            string sqlSelect = "SELECT * FROM petition_type";

            try
            {
                using (conn)
                {
                    var petitionTypes = await conn.QueryAsync<PetitionTypeDataResponse>(sqlSelect);

                    if (petitionTypes.Any())
                    {
                        response.Error.IsSuccess = true;
                        response.Data = petitionTypes.ToList();
                    }
                    else
                    {
                        response.Error.IsSuccess = false;
                        response.Error.ErrorType = "Data Base Error";
                        response.Error.ErrorMessage = "Couldn´t get data to make list from data base";
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

        // Update a petition type.
        internal async Task<ErrorResponse> UpdatePetitionType(UpdatePetitionTypeRequest request)
        {
            ErrorResponse response = new ErrorResponse();
            string sqlSelect = "SELECT `value` FROM petition_type WHERE `id` = @Id;";
            string sqlUpdate = "UPDATE petition_type SET Value = @Value, Selectable = @Selectable WHERE Id = @Id";

            try
            {
                using (conn)
                {
                    var Value = await conn.QueryAsync(sqlSelect, new { Id = request.Id }); // Checks if the petition type exists

                    if (Value.Count() != 0) // If exists we update petition type
                    {
                        var petitionType = await conn.ExecuteAsync(sqlUpdate, request);

                        if (petitionType != 0)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.ErrorType = "Data Base Error";
                            response.ErrorMessage = "No petition type updated in Data Base";
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.ErrorType = "Doesn´t Exists";
                        response.ErrorMessage = $"Can´t update petition type because the {request.Value} rol doesn´t exists";
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

        // Delete a petition type.
        internal async Task<ErrorResponse> DeletePetitionType(int petitionTypeId)
        {
            ErrorResponse response = new ErrorResponse();
            string sqlSelect = "SELECT `value` FROM petition_type WHERE `id` = @Id;";
            string sqlDelete = $"DELETE FROM petition_type WHERE `id` = @Id;;";

            try
            {
                using (conn)
                {
                    var Value = await conn.QueryAsync(sqlSelect, new { Id = petitionTypeId }); // Checks if the petition type exists

                    if (Value.Count() != 0) // If exists we delete petition type
                    {
                        var petitionType = await conn.ExecuteAsync(sqlDelete, new { Id = petitionTypeId });

                        if (petitionType != 0)
                        {
                            response.IsSuccess = true;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.ErrorType = "Data Base Error";
                            response.ErrorMessage = "No petition type deleted in Data Base";
                        }
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.ErrorType = "Doesn´t Exists";
                        response.ErrorMessage = $"Can´t delete rol because the id: {petitionTypeId} doesn´t exist.";
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
