using Dapper;
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

        public MonthlyDataDumpRepository(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetValue<string>("ConnectionStrings:Develop");
            conn = new MySqlConnection(connectionString);
        }

        public async Task<bool> EmployeeExists(string? userId)
        {
            if (userId == null) return false;
            else
            {
                return conn.Query<string>(String.Format("SELECT id_employee FROM employees WHERE id_employee = '{0}'", userId)).FirstOrDefault() != null;
            }
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

        public async Task<List<IncurredHoursByService>> GetIncurredHoursByService(string? leader_id, string? employee_id, string? service)
        {
            StringBuilder queryBuilder = new StringBuilder
                (
                @"
                SELECT 
	                leaders.id_employee AS id_leader,
	                not_leaders.id_employee, 
	                not_leaders.name AS employee_name,
	                hours_diff.total_diff AS remaining_hours
                FROM 
	                (
		                SELECT * 
		                FROM employees 
		                WHERE category LIKE '%Leader%' OR category LIKE '%Manager%' OR category LIKE '%Director%'
	                ) AS leaders 
	                INNER JOIN
	                (
		                SELECT * 
		                FROM employees 
		                WHERE category NOT LIKE '%Leader%' AND category NOT LIKE '%Manager%' AND category NOT LIKE '%Director%'
	                ) AS not_leaders
	                ON leaders.service_team = not_leaders.service_team AND leaders.service = not_leaders.service
	                INNER JOIN 
	                (
		                SELECT id_employee, ROUND((ROUND(SUM(REPLACE(total_hours, ',', '.')), 4) - ROUND(SUM(REPLACE(total_incurred_hours, ',', '.')), 4)), 4) AS total_diff
		                FROM monthly_incurred_hours
		                GROUP BY id_employee
	                ) AS hours_diff
	                ON not_leaders.id_employee = hours_diff.id_employee
                    WHERE hours_diff.total_diff > 0
                "
                );
            if(leader_id != null || employee_id != null || service != null)
            {
                StringBuilder paramsBuilder = new StringBuilder();
                if(leader_id != null)
                {
                    paramsBuilder.Append(String.Format( " AND leaders.id_employee = '{0}'", leader_id));
                }

                if(employee_id != null)
                {
                    paramsBuilder.Append(String.Format(" AND not_leaders.id_employee = '{0}'", employee_id));
                }

                if (service != null)
                {
                    paramsBuilder.Append(String.Format(" AND not_leaders.service = '{0}'", service));
                }

                queryBuilder.Append(paramsBuilder.ToString());
            }

            return conn.Query<IncurredHoursByService>(queryBuilder.ToString()).ToList();
        }

        public async Task<List<EmployeeRemainingHours>> GetRemainingIncurredHours(string month, string year, string? userId = null)
        {
            StringBuilder queryBuilder = new StringBuilder
                (
                    String.Format(
                        @"
                        SELECT DISTINCT
	                    e.id_employee, 
	                    e.name, 
	                    gi.service_team,
	                    (
		                    SELECT 
			                    CASE
				                    WHEN ROUND(SUM(REPLACE(i.incurred_hours, ',', '.')), 4) IS NULL THEN 0
				                    ELSE ROUND(SUM(REPLACE(i.incurred_hours, ',', '.')), 4)
			                    END
		                    FROM incurred i
		                    WHERE e.id_employee = i.id_employee 
			                    AND gi.service_team = i.service_team 
			                    AND MONTH(STR_TO_DATE(i.date, '%d/%m/%Y')) = '{0}'
                                AND YEAR(STR_TO_DATE(i.date, '%d/%m/%Y')) = '{1}'
	                    ) AS incurred_hours_by_team,
	                    ROUND((m.total_hours - m.total_incurred_hours), 4) AS remaining_incurred_hours
	                    FROM employees e 
		                    INNER JOIN monthly_incurred_hours m ON e.id_employee = m.id_employee
		                    LEFT JOIN incurred gi ON e.id_employee = gi.id_employee
	                    WHERE m.month = '{0}' AND m.year = '{1}'",
                    month, year)
                );

            if (userId != null) 
                queryBuilder.Append(String.Format(" AND e.id_employee = '{0}'", userId));
            
            return conn.Query<EmployeeRemainingHours>(queryBuilder.ToString()).ToList();
        }

        public async Task<List<EmployeeMonthlyIncurredHours>> GetTotalIncurredHoursByDate(string month, string year, string? userId)
        {
            StringBuilder query = new StringBuilder(

                String.Format(
                @"
                    SELECT e.id_employee, e.name, m.total_incurred_hours
                    FROM employees e INNER JOIN monthly_incurred_hours m ON e.id_employee = m.id_employee
                    WHERE m.month = '{0}' AND m.year = '{1}'
                ", month, year)
            );

            if(userId != null)
                query.Append(String.Format(" AND e.id_employee = '{0}'", userId));
                
            return conn.Query<EmployeeMonthlyIncurredHours>(query.ToString()).ToList();
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

            string insertParams = "('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";

            foreach (Incurred incurred in incurreds)
            {
                string insert = String.Format(insertParams + ((incurreds.Last() != incurred) ? "," : ""), incurred.id_employee, incurred.service_team, incurred.task_id, incurred.task_summary, incurred.incurred_hours, incurred.date, incurred.month_date);
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
