using Dapper;
using MySqlConnector;
using NTTLapso.Models.Login;
using NTTLapso.Models.UserSchedule;

namespace NTTLapso.Repository
{
    public class MastersRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public MastersRepository() { 
            conn = new MySqlConnection(connectionString);
        }

        public async Task<int> SetUserSchedule(UserScheduleSetRequest userScheduleRequest)
        {
            string checkIfExist = "SELECT `Value` FROM user_schedule WHERE `Value` = '" + userScheduleRequest.Value+"';";
           var value =  conn.Query<UserScheduleDataResponse>(checkIfExist).FirstOrDefault();
            string SQLQuery = "INSERT INTO user_schedule (`Value`) VALUES ('" + userScheduleRequest.Value + "');";
            int response = 0;
            if (value == null)
            {
                response = conn.Execute(SQLQuery);
                return response;
            }
            else
            {
                response = -2;
                return response;
            }
        }

        public async Task<UserScheduleDataResponse?> GetUserSchedule(UserScheduleRequest userScheduleRequest)
        {
            
            UserScheduleDataResponse response = new UserScheduleDataResponse();
            string SQLQuery = "SELECT Id, `Value` FROM user_schedule WHERE Id = " + userScheduleRequest.Id;
            response = conn.Query<UserScheduleDataResponse>(SQLQuery).FirstOrDefault();
            return response;
        }

        public async Task<int> DeleteUserSchedule(UserScheduleRequest userScheduleRequest)
        {
            int response = 0;
            string SQLQuery = "DELETE FROM user_schedule WHERE Id = " + userScheduleRequest.Id;
            response = conn.Execute(SQLQuery);
            return response;
        }

        public async Task<int> UpdateUserSchedule(UserScheduleRequest userScheduleRequest)
        {
            int response = 0;
            string SQLQuery = "UPDATE user_schedule SET `Value` = '" + userScheduleRequest.Value +  "' WHERE Id = " + userScheduleRequest.Id;
            response = conn.Execute(SQLQuery);
            return response;
        }
    }
}
