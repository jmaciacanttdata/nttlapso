using Dapper;
using Microsoft.SharePoint.Client.Discovery;
using Microsoft.SharePoint.Client;
using MySqlConnector;
using NTTLapso.Models.DataDump;
using NTTLapso.Tools;

using System.Text;

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

        public async Task<List<Employee>> GetUsers()
        {
            string sql = "SELECT id_employee FROM employees WHERE LENGTH(id_employee)>0 GROUP BY id_employee";

            return (conn.Query<Employee>(sql)).ToList();
        }

        public async Task<string> GetTotalHours(string? userId)
        {
            try
            {
                string sql ="SELECT ROUND(SUM(REPLACE(hours, ',', '.')), 4) FROM schedules s ";

                sql += string.Format("WHERE s.id_employee = {0}", userId);

                string sum = (conn.Query<string>(sql)).FirstOrDefault();

                return (sum != null && sum != "") ? sum : "0";
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
                string sql = "SELECT ROUND(SUM(REPLACE(incurred_hours, ',', '.')), 4) FROM incurred i ";

                sql += string.Format("WHERE i.id_employee = {0} AND MONTH(STR_TO_DATE(date, '%d/%m/%Y')) = {1}", userId, month);

                string sum = (conn.Query<string>(sql)).FirstOrDefault();

                return (sum != null && sum != "") ? sum : "0";
            }
            catch (Exception ex)
            {
                return "0";
            }
        }

        public async Task CreateCalculated(MonthlyIncurredHours monthlyIncurred)
        {
            string sqlInsert = String.Format(
                @"
                    INSERT INTO monthly_incurred_hours 
                    VALUES ('{0}','{1}','{2}','{3}','{4}')
                ", 
                monthlyIncurred.IdEmployee, monthlyIncurred.Year, monthlyIncurred.Month, monthlyIncurred.TotalHours, monthlyIncurred.TotalIncurredHours);
            conn.Query(sqlInsert);
        }

        private void TruncateTable(string tableName, bool enableFkChecks = false)
        {
            StringBuilder sb = new StringBuilder();
            if (enableFkChecks) sb.Append("SET FOREIGN_KEY_CHECKS = 0;");
            sb.Append("TRUNCATE TABLE " + tableName+";");
            if (enableFkChecks) sb.Append("SET FOREIGN_KEY_CHECKS = 1;");

            conn.Query(sb.ToString());
        }

        public async Task TruncateEmployees()
        {
            string tableName = "employees";
            TruncateTable(tableName, true);
        }

        public async Task TruncateSchedules()
        {
            string tableName = "schedules";
            TruncateTable(tableName);
        }

        public async Task TruncateIncurred()
        {
            string tableName = "incurred";
            TruncateTable(tableName);
        }

        public async Task InsertEmployees(List<Employee> employees)
        {
            StringBuilder sqlInsert = new StringBuilder("SET FOREIGN_KEY_CHECKS = 0; ");

            sqlInsert.Append("INSERT INTO employees VALUES ");
            string insertParams = "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}')";

            foreach (Employee employee in employees)
            {
                string insert = String.Format(insertParams + ((employees.Last() != employee) ? "," : ""), employee.id_employee, employee.name, employee.office, employee.hub, employee.micro_hub, employee.incorporation_date, employee.leave_date, employee.category, employee.business_unit, employee.division, employee.department, employee.service, employee.service_team, employee.asignation, employee.internal_area, employee.sector, employee.schedule, employee.workday_distribution, employee.reduced_workday, employee.days_intensive, employee.days_remote, employee.remote_schedule, employee.email, employee.tecnologic_lane, employee.tecnology, employee.coe, employee.study);
                sqlInsert.Append(insert);
            }

            sqlInsert.Append("; SET FOREIGN_KEY_CHECKS = 1;");
            conn.Query(sqlInsert.ToString());

        }

        public async Task InsertSchedules(List<Schedule> schedules)
        {
            StringBuilder sqlInsert = new StringBuilder("SET FOREIGN_KEY_CHECKS = 0; ");
 
            sqlInsert.Append("INSERT INTO schedules VALUES ");
            string insertParams = "('{0}','{1}','{2}')";

            foreach (Schedule schedule in schedules)
            {
                string insert = String.Format(insertParams + ((schedules.Last() != schedule) ? "," : ""), schedule.id_employee, schedule.date, schedule.hours);
                sqlInsert.Append(insert);
            }

            sqlInsert.Append("; SET FOREIGN_KEY_CHECKS = 1;");
            conn.Query(sqlInsert.ToString());
        }

        public async Task InsertIncurreds(List<Incurred> incurreds)
        {
            StringBuilder sqlInsert = new StringBuilder("SET FOREIGN_KEY_CHECKS = 0; ");
            sqlInsert.Append("INSERT INTO incurred VALUES ");

            string insertParams = "('{0}','{1}','{2}','{3}','{4}','{5}')";

            foreach (Incurred incurred in incurreds)
            {
                string insert = String.Format(insertParams + ((incurreds.Last() != incurred) ? "," : ""), incurred.id_employee, incurred.task_id, incurred.task_summary, incurred.incurred_hours, incurred.date, incurred.month_date);
                sqlInsert.Append(insert);
            }

            sqlInsert.Append("; SET FOREIGN_KEY_CHECKS = 1;");
            conn.Query(sqlInsert.ToString());
        }
    
        public async Task<int> CreateConsolidation()
        {
            conn.Execute("CALL createConsolidation;");

            string query =
                @"
                SELECT COUNT(sc.id_employee) 
                FROM (
	                SELECT c.id_employee, c.name, c.service, c.service_team, c.NotEmployees, c.NotSchedules, c.NotIncurred
	                FROM consolidation c
	                WHERE c.id_employee IS NOT NULL
	                GROUP BY id_employee, NAME, service, service_team
                ) sc   
                ";
            return conn.Query<int>(query).FirstOrDefault();
        }

        public async Task<List<Consolidation>> GetConsolidatedEmployees()
        {
            string query =
                @"
                    SELECT c.id_employee, c.name, c.service, c.service_team, c.NotEmployees, c.NotSchedules, c.NotIncurred
                    FROM consolidation c
                    WHERE c.id_employee IS NOT NULL
                    GROUP BY id_employee, NAME, service, service_team
                ";

            return conn.Query<Consolidation>(query).ToList();
        }
    }
}
