using Dapper;
using MySqlConnector;
using NTTLapso.Models.DataDump;

namespace NTTLapso.Repository
{
    public class MonthlyDataDumpRepository
    {
        private static string connectionString = "";
        private MySqlConnection conn;
        private IConfiguration _config;
        public MonthlyDataDumpRepository(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetValue<string>("ConnectionStrings:Develop");
            conn = new MySqlConnection(connectionString);
        }

        public async Task<List<string>> GetUser(string? id=null)
        {
            string sql = "SELECT id_employee FROM employees WHERE LENGTH(id_employee)>0";

            if (id == null)
            {
                return (conn.Query<string>(sql)).ToList();
            }
            else
            {
                sql += String.Format(" AND id_employee = {0}", id);
                return (conn.Query<string>(sql)).ToList();
            }
        }

        public async Task<List<Employee>> GetAllDataUser(string? id = null)
        {
            string sql = "SELECT * FROM employees WHERE LENGTH(id_employee)>0";

            if (id == null)
            {
                return (conn.Query<Employee>(sql)).ToList();
            }
            else
            {
                sql += String.Format(" AND id_employee = {0}", id);
                return (conn.Query<Employee>(sql)).ToList();
            }
        }

        public async Task<float> GetTotalHoras(string? userId)
        {
            try
            {
                string sql =
                @"
                    SELECT SUM(hours) 
                    FROM schedules s
                ";
                sql += string.Format("WHERE s.id_employee = {0}", userId);

                return conn.Query<float>(sql).FirstOrDefault();

            }catch(Exception ex)
            {
                return 0.0f;
            }
        }

        public async Task<float> GetIncurred(string? userId)
        {
            try
            {
                string sql =
                    @"
                        SELECT SUM(incurred_hours) 
                        FROM incurred i
                    ";

                sql += string.Format("WHERE i.id_employee = {0}", userId);

                return (conn.Query<float>(sql)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return 0.0f;
            }
        }

        public async Task CreateCalculated(MonthlyIncurredHours monthlyIncurred)
        {

            string sqlInsert = String.Format("INSERT INTO monthly_incurred_hours(id_employee, year, month, total_hours, total_incurred_hours) VALUES ({0},{1},{2},{3},{4})", monthlyIncurred.NumEmpleado, monthlyIncurred.Anyo, monthlyIncurred.Mes, monthlyIncurred.HorasIncurrir, monthlyIncurred.HorasIncurridas);
            conn.Query(sqlInsert);
        }
    }
}
