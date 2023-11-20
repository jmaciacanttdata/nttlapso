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

        public async Task CloneTest()
        {
            if (_testStr == "") return;

            try
            {
                string cloneQuery   = "DROP TABLE IF EXISTS " + _testStr + "employees;";
                cloneQuery          += " CREATE TABLE " + _testStr + "employees AS SELECT * FROM employees;";
                conn.Execute(cloneQuery);

                cloneQuery          = " DROP TABLE IF EXISTS " + _testStr + "schedules;";
                cloneQuery          += " CREATE TABLE " + _testStr + "schedules AS SELECT * FROM schedules;";
                conn.Execute(cloneQuery);

                cloneQuery          = " DROP TABLE IF EXISTS " + _testStr + "incurred;";
                cloneQuery          += " CREATE TABLE " + _testStr + "incurred AS SELECT * FROM incurred;";
                conn.Execute(cloneQuery);

                cloneQuery          = " DROP TABLE IF EXISTS " + _testStr + "monthly_incurred_hours;";
                cloneQuery          += " CREATE TABLE " + _testStr + "monthly_incurred_hours AS SELECT * FROM monthly_incurred_hours;";
                conn.Execute(cloneQuery);
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error al clonar las tablas de test.");
            }
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

        public async Task InsertEmployees(List<Employee> employees)
        {
            string sqlInsert = "";
            string sqlInsertParams = "";

            try
            {
                sqlInsert = "INSERT INTO "+_testStr+"employees VALUES ";
                for (int i = 0; i < 26; i++)
                {
                    sqlInsertParams += "'{" + i + "}',";
                }

                sqlInsertParams += "'{26}'";

                for(int i = 0; i < employees.Count; i++)
                {
                    Employee employee = employees[i];

                    string incr_date = DateTime.Parse(employee.incorporation_date).ToShortDateString();

                    sqlInsert += String.Format("(" + sqlInsertParams + ")"+((i != employees.Count -1) ? ",":""),
                        employee.id_employee, employee.name, employee.office, employee.hub, employee.micro_hub, incr_date, employee.leave_date, employee.category, employee.business_unit,
                        employee.division, employee.department, employee.service, employee.service_team, employee.asignation, employee.internal_area, employee.sector, employee.schedule, employee.workday_distribution, employee.reduced_workday, employee.days_intensive,
                        employee.days_remote, employee.remote_schedule, employee.email, employee.tecnologic_lane, employee.tecnology, employee.coe, employee.study);
                }

                conn.Query(sqlInsert);
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error a la hora de insertar los empleados del excel en la base de datos.");
            }

        }

        public async Task InsertSchedules(List<Schedule> schedules)
        {
            try
            {
                List<Schedule> filtered = schedules.FindAll((schedule) => schedule.id_employee != null);
                string sqlInsert = "INSERT INTO "+_testStr+"schedules VALUES ";
                for(int i = 0; i < filtered.Count; i++)
                {
                    Schedule schedule = filtered[i];

                    string date = DateTime.Parse(schedule.date).ToShortDateString();
                    sqlInsert += String.Format("('{0}', '{1}', '{2}')" + ((i != filtered.Count - 1) ? "," : ""), schedule.id_employee, date, schedule.hours);
                }

                conn.Query(sqlInsert);
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error a la hora de insertar los horarios del excel en la base de datos."+ e.Message);
            }

        }

        public async Task InsertIncurred(List<Incurred> incurreds)
        {
            string sqlInsert = "";

            try
            {
                sqlInsert = "INSERT INTO "+_testStr+"incurred VALUES ";
                string sqlInsertParams = "";
                for (int i = 0; i < 27; i++)
                {
                    sqlInsertParams += "{" + i + "},";
                }

                sqlInsertParams += "\"{27}\"";

                int maxElements = 1000;

                for (int i = 0; i < maxElements; i++)
                {
                    Incurred incurred = incurreds[i];
                    string date = DateTime.Parse(incurred.fecha).ToShortDateString();
                    string taskSumm = incurred.task_summary.Replace("\'", "");
                    string incurredComm = incurred.incurred_comment.Replace("\'", "()");

                    sqlInsert += String.Format("(" + sqlInsertParams + ")" + ((i != incurreds.Count - 1) ? "," : ""), 
                        incurred.id_employee, incurred.name, incurred.situacion_actual_persona, incurred.pkey_jira, incurred.component, incurred.group, incurred.service_line, incurred.task_type, incurred.facturable, 
                        incurred.task_id, incurred.task_summary, incurred.task_status, incurred.task_origin, incurred.internal_estimation, incurred.agile_estimation, incurred.estimation_unit, incurred.sub_task_type, incurred.typology,
                        incurred.sub_task_id, taskSumm, incurred.sub_task_status, incurred.sub_task_origin, incurredComm, incurred.sub_task_estimation, incurred.incurred_hours, incurred.etc, date, incurred.month_date);
              
                }

                conn.Query(sqlInsert);
            }
            catch (Exception e)
            {
                throw new DataDumpException("Error: Error a la hora de insertar los incurridos del excel en la base de datos. "+sqlInsert + "  " + e.Message);
            }

        }
    }
}
