using Dapper;
using MySqlConnector;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;

namespace NTTLapso.Repository
{
    public class MastersRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public MastersRepository() { 
            conn = new MySqlConnection(connectionString);
        }

        public async Task<int> PetitionRegister(PetitionStatusSetRequest petitionStatusRequest)
        {
            int response = 0;
            string SQLQuery = "INSERT INTO petition_state (`Value`, `IdTextNotification`) VALUES ('" + petitionStatusRequest.Value + "', '" + petitionStatusRequest.IdTextNotification + "');";
            response = conn.Execute(SQLQuery);
            return response;
        }

        public async Task<PetitionStatusDataResponse?> GetPetitionStatus(PetitionStatusRequest petitionStatusRequest)
        {
            PetitionStatusDataResponse response = new PetitionStatusDataResponse();
            string SQLQuery = "SELECT Id, `Value`, `IdTextNotification` FROM petition_state WHERE Id = " + petitionStatusRequest.Id;
            response = conn.Query<PetitionStatusDataResponse>(SQLQuery).FirstOrDefault();
            return response;
        }

        public async Task<int> DeletePetitionStatus(PetitionStatusRequest petitionStatusRequest)
        {
            int response = 0;
            string SQLQuery = "DELETE FROM petition_state WHERE Id = " + petitionStatusRequest.Id;
            response = conn.Execute(SQLQuery);
            return response;
        }

        public async Task<int> UpdatePetitionStatus(PetitionStatusRequest petitionStatusRequest)
        {
            int response = 0;
            string SQLQuery = "UPDATE permission SET `Value` = '" + petitionStatusRequest.Value + "' , `IdTextNotification` = " + petitionStatusRequest.IdTextNotification +  " WHERE Id = " + petitionStatusRequest.Id;
            response = conn.Execute(SQLQuery);
            return response;
        }
    }



}
