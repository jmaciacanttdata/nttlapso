using Dapper;
using MySqlConnector;
using NTTLapso.Models.Process.UserCharge;

namespace NTTLapso.Repository
{
    public class ProcessRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public ProcessRepository()
        {
            conn = new MySqlConnection(connectionString);
        }

        // Method for inserting user's vacation and compensated days at beginning of year.
        public async Task SetUsersCharge(UserChargeRequest request)
        {
            string SQLQueryGeneral = String.Format("INSERT INTO user_charge(`IdUser`, `Year`, `TotalVacationDays`, `TotalCompensatedDays`) VALUES({0}, {1}, {2}, {3})", request.IdUser, request.Year, request.TotalVacationDays, request.TotalCompensatedDays);
            await conn.QueryAsync(SQLQueryGeneral);
        }

        // Method for inserting new user's vacation and compensated days for the remainder of the year.
        public async Task SetNewUserCharge(UserChargeRequest request)
        {
            string SQLQuery = String.Format("SELECT `IdUser` FROM user_charge WHERE `IdUser` = {0} AND `Year` = {1};", request.IdUser, request.Year);

            var response = await conn.QueryAsync(SQLQuery); // Check if user charge exists

            if (response.Count() == 0) // If doesn't exist we insert user charge.
            {
                string SQLQueryGeneral = String.Format("INSERT INTO user_charge(`IdUser`, `Year`, `TotalVacationDays`, `TotalCompensatedDays`) VALUES({0}, {1}, {2}, {3})", request.IdUser, request.Year, request.TotalVacationDays, request.TotalCompensatedDays);
                await conn.QueryAsync(SQLQueryGeneral);
            }
            else
            {
                throw new Exception(message: $"The user with id: {request.IdUser} already has a charge asigned for the year: {request.Year}");
            }
        }
    }
}
