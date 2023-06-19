using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.PetitionStatus;
using NTTLapso.Models.PetitionType;
using System.Web.WebPages;

namespace NTTLapso.Repository
{
    public class PetitionTypeRepository
    {
        private static string connectionString;
        private MySqlConnection conn;
        private IConfiguration _config;
        public PetitionTypeRepository(IConfiguration config)
        {
            _config = config;
            //conn = new MySqlConnection(connectionString);
            connectionString = _config.GetValue<string>("ConnectionStrings:Develop");
            conn = new MySqlConnection(connectionString);
            //int compensatedDays = _config.GetValue<int>("UserCharge:CompensatedDays");
        }

        // Get petition type list.
        public async Task<List<PetitionTypeDataResponse>> List(PetitionTypeRequest? request)
        {
            List<PetitionTypeDataResponse> response = new List<PetitionTypeDataResponse>();
            string SQLQueryGeneral = "SELECT Id, Value, Selectable FROM petition_type WHERE 1=1";

            if (request != null && request.Id > 0)
            {
                SQLQueryGeneral += " AND Id={0}";
            }
            if (request != null && request.Value != null && request.Value != "")
            {
                SQLQueryGeneral += " AND Value LIKE '%{1}%'";
            }
            if (request != null && request.Selectable != null && request.Selectable == true)
            {
                SQLQueryGeneral += " AND Selectable = 1";
            }
            else if (request != null && request.Selectable != null && request.Selectable == false)
            {
                SQLQueryGeneral += " AND Selectable = 0";
            }

            string SQLQuery = String.Format(SQLQueryGeneral, request.Id, request.Value);
            response = (await conn.QueryAsync<PetitionTypeDataResponse>(SQLQuery)).ToList();

            if (response.Count == 0) 
            {
                throw new Exception(message: "There are no data in the database or there are none matching the applied filter.");
            }

            return response;
        }

        // Create new petition type.
        public async Task Create(CreatePetitionTypeRequest request)
        {
            string sqlSelect = String.Format("SELECT `value` FROM petition_type WHERE LOWER(`value`) = LOWER('{0}');", request.Value);
            var query = await conn.QueryAsync(sqlSelect); // Checks if the petition type already exists.

            if (query.Count() == 0) // If doesn´t exist we insert petition type.
            {
                string SQLQueryGeneral = String.Format("INSERT INTO petition_type(Value, Selectable) VALUES('{0}', {1})", request.Value, request.Selectable);
                await conn.QueryAsync(SQLQueryGeneral);
            }
            else
            {
                throw new Exception(message: $"The '{request.Value}' type already exists in the database.");
            }
        }

        // Edit a petition type.
        public async Task Edit(PetitionTypeRequest request)
        {
            string sqlSelect = String.Format("SELECT `id` FROM petition_type WHERE id = {0};", request.Id);
            var query = await conn.QueryAsync(sqlSelect); // Checks if the petition type exists.

            if (query.Count() != 0) // If exists we update petition type.
            {
                string SQLQueryGeneral = String.Format("UPDATE petition_type SET Value='{1}', Selectable={2} WHERE Id={0}", request.Id, request.Value, request.Selectable);
                await conn.QueryAsync(SQLQueryGeneral);
            }
            else
            {
                throw new Exception(message: $"The '{request.Value}' type can't be updated because it doesn't exist in the database.");
            }
        }

        // Delete a petition type
        public async Task Delete(int Id)
        {
            string sqlSelect = String.Format("SELECT `id` FROM petition_type WHERE id = {0};", Id);
            var query = await conn.QueryAsync(sqlSelect); // Checks if the petition type exists.

            if (query.Count() != 0) // If exists we update petition type.
            {
                string SQLQueryGeneral = String.Format("DELETE FROM petition_type WHERE Id={0};", Id);
                await conn.QueryAsync(SQLQueryGeneral);
            }
            else
            {
                throw new Exception(message: $"The type with id {Id} can't be deleted because it doesn't exist in the database.");
            }
        }
    }
}
