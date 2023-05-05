using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
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
    }
}
