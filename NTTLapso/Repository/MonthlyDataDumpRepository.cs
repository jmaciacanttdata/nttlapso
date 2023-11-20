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

        private string _testStr;

        public MonthlyDataDumpRepository(IConfiguration config, bool test = false)
        {
            _config = config;
            connectionString = _config.GetValue<string>("ConnectionStrings:Develop");
            conn = new MySqlConnection(connectionString);

            _testStr = test ? "test_" : "";
        }

        public async Task<List<string>> GetUser(string? id=null)
        {
            string sql = "SELECT id_employee FROM "+_testStr+ "employees WHERE LENGTH(id_employee)>0";

            if (id != null)
            {
                sql += String.Format(" AND id_employee = {0}", id);
            }

            sql += " GROUP BY id_employee";

            return (conn.Query<string>(sql)).ToList();
        }

        public async Task<List<Employee>> GetAllDataUser(string? id = null)
        {
            string sql = "SELECT * FROM " +_testStr+ "employees WHERE LENGTH(id_employee)>0";

            if (id != null)
            {
                sql += String.Format(" AND id_employee = {0}", id);
            }

            sql += " GROUP BY id_employee";

            return (conn.Query<Employee>(sql)).ToList();
        }

        public async Task<string> GetTotalHoras(string? userId)
        {
            try
            {
                string sql ="SELECT ROUND(SUM(REPLACE(hours, ',', '.')), 4) FROM "+_testStr+"schedules s ";

                sql += string.Format("WHERE s.id_employee = {0}", userId);

                return conn.Query<string>(sql).FirstOrDefault();
            }
            catch(Exception ex)
            {
                return "0";
            }
        }

        public async Task<string> GetIncurred(string? userId, string month)
        {
            try
            {
                string sql = "SELECT ROUND(SUM(REPLACE(incurred_hours, ',', '.')), 4) FROM " + _testStr+"incurred i ";

                sql += string.Format("WHERE i.id_employee = {0} AND MONTH(STR_TO_DATE(date, '%d/%m/%Y')) = {1}", userId, month);

                return (conn.Query<string>(sql)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public async Task CreateCalculated(MonthlyIncurredHours monthlyIncurred)
        {

            string sqlInsert = String.Format("INSERT INTO "+_testStr+"monthly_incurred_hours(id, year, month, total_hours, total_incurred_hours) VALUES ({0},{1},{2},{3},{4})", monthlyIncurred.IdEmployee, monthlyIncurred.Year, monthlyIncurred.Month, monthlyIncurred.TotalHours, monthlyIncurred.TotalIncurredHours);
            conn.Query(sqlInsert);
        }

        private void TruncateTable(string tableName)
        {
            try
            {
                string sqlTruncate = "TRUNCATE TABLE " + tableName;
                conn.Query(sqlTruncate);
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Se ha producido un error a la hora de truncar la tabla " + tableName);
            }
        }

        public async Task TruncateEmployees()
        {
            string tableName = _testStr + "employees";
            TruncateTable(tableName);
        }

        public async Task TruncateSchedules()
        {
            string tableName = _testStr + "schedules";
            TruncateTable(tableName);
        }

        public async Task TruncateIncurred()
        {
            string tableName = _testStr + "incurred";
            TruncateTable(tableName);
        }

        public async Task InsertEmployees(string employeesInsert)
        {
            string sqlInsert = "";

            try
            {
                sqlInsert += "INSERT INTO "+_testStr+"employees VALUES "+employeesInsert;
                conn.Query(sqlInsert);
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error a la hora de insertar los empleados del excel en la base de datos." + sqlInsert + " " + e.Message);
            }

        }

        public async Task InsertSchedules(string schedulesInsert)
        {
            string sqlInsert = "";

            try
            {
                sqlInsert = "INSERT INTO "+_testStr+"schedules VALUES "+schedulesInsert;
                conn.Query(sqlInsert);
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error a la hora de insertar los horarios del excel en la base de datos."+ sqlInsert + " " + e.Message);
            }

        }

        public async Task InsertIncurred(string incurredsInsert)
        {
            string sqlInsert = "";

            try
            {
                sqlInsert = "INSERT INTO "+_testStr+"incurred VALUES "+incurredsInsert;
                conn.Query(sqlInsert);
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error a la hora de insertar los incurridos del excel en la base de datos. "+sqlInsert + "  " + e.Message);
            }

        }
    }
}
