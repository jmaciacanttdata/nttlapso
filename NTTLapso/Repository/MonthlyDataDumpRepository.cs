using Dapper;
using Microsoft.SharePoint.Client.Discovery;
using Microsoft.SharePoint.Client;
using MySqlConnector;
using NTTLapso.Models.DataDump;

using System.Text;

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

        public async Task InsertIntoTryEmployees(List<Employee> employees)
        {
            StringBuilder sqlInsert = new StringBuilder("INSERT INTO try_employees VALUES ");

            try
            {
                TruncateTable("try_employees");

                foreach (Employee employee in employees)
                {
                    bool isLast = employees.Last() != employee;
                    string employeeQueryInsert = String.Format("('{0}', '{1}', '{2}')", employee.id_employee, employee.name, employee.service_team);
                    employeeQueryInsert += isLast ? "," : "";
                    sqlInsert.Append(employeeQueryInsert);
                }
                conn.Query(sqlInsert.ToString());
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error a la hora de insertar los empleados del excel en la base de datos." + sqlInsert + " " + e.Message);
            }

        }

        public async Task InsertIntoTrySchedules(List<Schedule> schedules)
        {

            StringBuilder sqlInsert = new StringBuilder("INSERT INTO try_schedules VALUES ");

            try
            {
                TruncateTable("try_schedules");

                foreach (Schedule schedule in schedules)
                {
                    bool isLast = schedules.Last() != schedule;
                    string scheduleQueryInsert = String.Format("('{0}', '{1}', '{2}')", schedule.id_employee, schedule.date, schedule.hours);
                    scheduleQueryInsert += isLast ? "," : "";
                    sqlInsert.Append(scheduleQueryInsert);
                }

                conn.Query(sqlInsert.ToString());
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error a la hora de insertar los horarios del excel en la base de datos."+ sqlInsert + " " + e.Message);
            }

        }

        public async Task InsertIntoTryIncurred(List<Incurred> incurreds)
        {
            StringBuilder sqlInsert = new StringBuilder("INSERT INTO try_incurred VALUES ");

            try
            {
                TruncateTable("try_incurred");

                foreach (Incurred incurred in incurreds)
                {
                    bool isLast = incurreds.Last() != incurred;
                    string incurredQueryInsert = String.Format("('{0}', '{1}', '{2}')", incurred.id_employee, incurred.incurred_hours, incurred.date);
                    incurredQueryInsert += isLast ? "," : "";
                    sqlInsert.Append(incurredQueryInsert);
                }
                conn.Query(sqlInsert.ToString());
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error a la hora de insertar los incurridos del excel en la base de datos. "+sqlInsert + "  " + e.Message);
            }

        }
    
        public async Task<List<QuarantineRelationshipData>> GetQuarantineRelationship()
        {
            try
            {
                string sqlSelect = @"SELECT DISTINCT all_employees.id_employee, 
                                    CASE
                                        WHEN try_employees.id_employee IS NULL AND try_incurred.id_employee IS NULL THEN 'No existe en employees e incurred'
                                        WHEN try_employees.id_employee IS NULL AND try_schedules.id_employee IS NULL THEN 'No existe en employees y schedules'
                                        WHEN try_incurred.id_employee IS NULL AND try_schedules.id_employee IS NULL THEN 'No existe en incurred y schedules'
                                        WHEN try_employees.id_employee IS NULL THEN 'No existe en employees'
                                        WHEN try_incurred.id_employee IS NULL THEN 'No existe en incurred'
                                        WHEN try_schedules.id_employee IS NULL THEN 'No existe en schedules'
                                    END AS condicion
                                FROM(SELECT id_employee FROM try_employees
                                      UNION SELECT id_employee FROM try_incurred
                                      UNION SELECT id_employee FROM try_schedules) AS all_employees
                                LEFT JOIN try_employees ON all_employees.id_employee = try_employees.id_employee
                                LEFT JOIN try_incurred ON all_employees.id_employee = try_incurred.id_employee
                                LEFT JOIN try_schedules ON all_employees.id_employee = try_schedules.id_employee
                                ";
                return conn.Query<QuarantineRelationshipData>(sqlSelect).ToList();
            }
            catch(Exception e)
            {
                throw new DataDumpException("Error a la hora de obtener los datos de cuarentena de la base de datos." + e.Message);
            }
        }
    }
}
