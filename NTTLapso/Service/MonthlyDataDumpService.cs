using NTTLapso.Models.DataDump;
using NTTLapso.Repository;
using NTTLapso.Tools;
using OfficeOpenXml;
using Tools;

namespace NTTLapso.Service
{
    public class MonthlyDataDumpService
    {
        private MonthlyDataDumpRepository _repo;
        private ExcelExtractor _excelExtractor;
        private SharePointDownloader _sharePointDownloader;

        private string _saveDirectory;

        public MonthlyDataDumpService(IConfiguration conf)
        {
            _repo = new MonthlyDataDumpRepository(conf, true);
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            _excelExtractor = new ExcelExtractor();

            string sharePointUrl = "https://everisgroup.sharepoint.com/sites/NTTLapsoTeam";
            string sharePointUser = "";
            string sharePointPassword = "";

            _sharePointDownloader = new SharePointDownloader(sharePointUrl, sharePointUser, sharePointPassword);

            _saveDirectory = Path.Combine(AppContext.BaseDirectory, "Excels\\");

            if (!Directory.Exists(_saveDirectory))
            {
                Directory.CreateDirectory(_saveDirectory);
            }
        }

        private async Task DoUserCalc(string userId)
        {
            MonthlyIncurredHours incurredUser = new MonthlyIncurredHours();
            incurredUser.IdEmployee = userId;
            incurredUser.Year = ((DateTime.Now.Month == 1) ? DateTime.Now.Year - 1 : DateTime.Now.Year).ToString();
            incurredUser.Month = ((DateTime.Now.Month == 1) ? 12 : DateTime.Now.Month - 1).ToString();
            incurredUser.TotalHours = await _repo.GetTotalHoras(userId);
            incurredUser.TotalIncurredHours = await _repo.GetIncurred(userId, incurredUser.Month);

            await _repo.CreateCalculated(incurredUser);
        }

        private Dictionary<string, Tuple<string, bool>> GetEmployeesColumns()
        {
            var columnas = new Dictionary<string, Tuple<string, bool>>();
            //NombreEnTabla<propiedad - importar> 
            columnas.Add("Numero Empleado", Tuple.Create("id_employee", true));
            columnas.Add("Persona", Tuple.Create("name", true));
            columnas.Add("Oficina", Tuple.Create("office", true));
            columnas.Add("Hub", Tuple.Create("hub", true));
            columnas.Add("Micro hub", Tuple.Create("micro_hub", true));
            columnas.Add("Fecha Incorporación", Tuple.Create("incorporation_date", true));
            columnas.Add("Fecha Baja", Tuple.Create("leave_date", true));
            columnas.Add("Categoria", Tuple.Create("category", true));
            columnas.Add("Bussines Unit", Tuple.Create("business_unit", true));
            columnas.Add("Division", Tuple.Create("division", true));
            columnas.Add("Department", Tuple.Create("department", true));
            columnas.Add("Servicio", Tuple.Create("service", true));
            columnas.Add("Service Team", Tuple.Create("service_team", true));
            columnas.Add("% Asignación", Tuple.Create("asignation", true));
            columnas.Add("Área Interna", Tuple.Create("internal_area", true));
            columnas.Add("Sector", Tuple.Create("sector", true));
            columnas.Add("Horario", Tuple.Create("schedule", true));
            columnas.Add("Distribución de jornada", Tuple.Create("workday_distribution", true));
            columnas.Add("Reducida", Tuple.Create("reduced_workday", true));
            columnas.Add("Dias Intensiva", Tuple.Create("days_intensive", true));
            columnas.Add("Dias Teletrabajo", Tuple.Create("days_remote", true));
            columnas.Add("Horario Teletrabajo", Tuple.Create("remote_schedule", true));
            columnas.Add("Email", Tuple.Create("email", true));
            columnas.Add("Línea Tecnológica", Tuple.Create("tecnologic_lane", true));
            columnas.Add("Tecnología", Tuple.Create("tecnology", true));
            columnas.Add("COE", Tuple.Create("coe", true));
            columnas.Add("Estudio", Tuple.Create("study", true));

            return columnas;
        }

        private Dictionary<string, Tuple<string, bool>> GetSchedulesColumns()
        {
            var columnasSchedule = new Dictionary<string, Tuple<string, bool>>();
            columnasSchedule.Add("numero_empleado", Tuple.Create("id_employee", true));
            columnasSchedule.Add("fecha", Tuple.Create("date", true));
            columnasSchedule.Add("horas", Tuple.Create("hours", true));

            return columnasSchedule;
        }

        private Dictionary<string, Tuple<string, bool>> GetIncurredColumns()
        {
            var columnasIncurred = new Dictionary<string, Tuple<string, bool>>();
            columnasIncurred.Add("Numero Empleado", Tuple.Create("id_employee", true));
            columnasIncurred.Add("Nombre Persona", Tuple.Create("name", true));
            columnasIncurred.Add("Situación actual persona", Tuple.Create("situacion_actual_persona", true));
            columnasIncurred.Add("Pkey Jira", Tuple.Create("pkey_jira", true));
            columnasIncurred.Add("Componente", Tuple.Create("component", true));
            columnasIncurred.Add("Agrupación", Tuple.Create("group", true));
            columnasIncurred.Add("Service Line", Tuple.Create("service_line", true));
            columnasIncurred.Add("Tipo Task", Tuple.Create("task_type", true));
            columnasIncurred.Add("Facturable a cliente", Tuple.Create("facturable", true));
            columnasIncurred.Add("Id Task", Tuple.Create("task_id", true));
            columnasIncurred.Add("Task Summary", Tuple.Create("task_summary", true));
            columnasIncurred.Add("Estado Task", Tuple.Create("task_status", true));
            columnasIncurred.Add("Origen Task", Tuple.Create("task_origin", true));
            columnasIncurred.Add("Estimación Interna", Tuple.Create("internal_estimation", true));
            columnasIncurred.Add("Estimacion Agile", Tuple.Create("agile_estimation", true));
            columnasIncurred.Add("Unidad Estimacion", Tuple.Create("estimation_unit", true));
            columnasIncurred.Add("Tipo Sub Task", Tuple.Create("sub_task_type", true));
            columnasIncurred.Add("Typology", Tuple.Create("typology", true));
            columnasIncurred.Add("Id Sub Task", Tuple.Create("sub_task_id", true));
            columnasIncurred.Add("Sub Task Summary", Tuple.Create("sub_task_summary", true));
            columnasIncurred.Add("Estado Sub Task", Tuple.Create("sub_task_status", true));
            columnasIncurred.Add("Origen Sub Task", Tuple.Create("sub_task_origin", true));
            columnasIncurred.Add("Comentario Incurrido", Tuple.Create("incurred_comment", true));
            columnasIncurred.Add("Estimacion Sub Task", Tuple.Create("sub_task_estimation", true));
            columnasIncurred.Add("Horas Incurridas", Tuple.Create("incurred_hours", true));
            columnasIncurred.Add("ETC", Tuple.Create("etc", true));
            columnasIncurred.Add("Fecha", Tuple.Create("fecha", true));
            columnasIncurred.Add("Fecha Mes", Tuple.Create("month_date", true));

            return columnasIncurred;
        }

        public async Task<DataDumpResponse> DumpData(string? userId = null)
        {
            DataDumpResponse result = new DataDumpResponse();
            List<string> Users = new List<string>();
            List<string> UsersFailed = new List<string>();

            try
            {
                Users = await _repo.GetUser(userId);
            }
            catch (Exception ex)
            {
                result.Completed = false;
                result.Message = "Error: Error al intentar obtener los usuarios.";
            }

            if (Users.Count() > 0 && result.Completed==null)
            {

                    foreach (string User in Users)
                    {
                        try
                        {
                            await DoUserCalc(User);
                        }
                        catch(Exception ex)
                        {
                            UsersFailed.Add(User);
                        }
                    }

                    if( UsersFailed.Count()==0)
                    {
                        result.Completed = true;
                        result.Message = "";
                    }
                    else
                    {
                        result.Completed = true;
                        result.Message = "Existen calculados de usuario sin registrar (revisar listado de empleados fallidos).";
                        foreach(string UFailed in UsersFailed)
                        {
                            List<Employee> e = new List<Employee>();
                            e = await _repo.GetAllDataUser(UFailed);
                            result.FailedEmployees.Add(e.FirstOrDefault());
                        }
                    }
            }

            return result;
        }
    


        public async Task<DataDumpResponse> LoadDataFromExcels()
        {
            DataDumpResponse resp = new DataDumpResponse();

            try
            {
                try
                {
                    /* AQUI SE REALIZA EL TRUNCADO DE DATOS */
                    //await _repo.TruncateIncurred();
                    await _repo.TruncateSchedules();
                    await _repo.TruncateEmployees();
                }
                catch (DataDumpException e)
                {
                    resp.Completed = false;
                    resp.Message = e.Message;
                    return resp;
                }

                // OBTENER EXCEL DE EMPLOYEES.
                _sharePointDownloader.Download("Documentos%20compartidos/General/Data/Headcount.xlsx", _saveDirectory);
                ExcelPackage excelEmployeesPackage = new ExcelPackage(Path.Combine(_saveDirectory, "Headcount.xlsx"));
                ExcelWorksheet excelSheetEmployees = excelEmployeesPackage.Workbook.Worksheets["Detalle"];
                List<Employee> employees = _excelExtractor.GetList<Employee>(excelSheetEmployees, GetEmployeesColumns());

                // OBTENER EXCEL DE SCHEDULES.
                _sharePointDownloader.Download("Documentos%20compartidos/General/Data/horarios/octubre_2023.xlsx", _saveDirectory);
                ExcelPackage excelSchedulesPackage = new ExcelPackage(Path.Combine(_saveDirectory, "octubre_2023.xlsx"));
                ExcelWorksheet excelSheetSchedules = excelSchedulesPackage.Workbook.Worksheets["Horarios"];
                List<Schedule> schedules = _excelExtractor.GetList<Schedule>(excelSheetSchedules, GetSchedulesColumns());

                // OBTENER EXCEL DE INCURRED
                /*
                _sharePointDownloader.Download("Documentos%20compartidos/General/Data/incurridos/Incurridos%20Periodo%20en%20curso.xlsx", _saveDirectory);
                ExcelPackage excelIncurredPackage = new ExcelPackage(Path.Combine(_saveDirectory, "Incurridos Periodo en curso.xlsx"));
                ExcelWorksheets worksheets = excelIncurredPackage.Workbook.Worksheets;
                ExcelWorksheet excelSheetIncurred = worksheets["Detalle Modificado"];
                List<Incurred> incurreds = _excelExtractor.GetList<Incurred>(excelSheetIncurred, GetIncurredColumns());
                */




                try
                {
                    await _repo.InsertEmployees(employees);
                    await _repo.InsertSchedules(schedules);
                    //await _repo.InsertIncurred(incurreds);

                    resp.Completed = true;
                    resp.Message = "Se ha completado el volcado de datos.";
                    return resp;
                }
                catch (DataDumpException e)
                {
                    resp.Completed = false;
                    resp.Message = e.Message;
                    return resp;
                }
            }
            catch (Exception e)
            {
                resp.Completed = false;
                resp.Message = "Error: Ha habido un error a la hora de cargar los excels."+ e.Message;

                return resp;
            }
        }
    }
}
 