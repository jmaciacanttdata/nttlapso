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

            _saveDirectory = Path.Combine(AppContext.BaseDirectory, "temp\\");

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

        private Dictionary<string, Tuple<bool, string, Func<string, string>?>> GetEmployeesColumns()
        {

            var columnas = new Dictionary<string, Tuple<bool, string, Func<string, string>?>>
            {
                { "Numero Empleado",            Tuple.Create<bool, string, Func<string, string>?>(true, "id_employee", null) },
                { "Persona",                    Tuple.Create<bool, string, Func<string, string>?>(true, "name", null) },
                { "Oficina",                    Tuple.Create<bool, string, Func<string, string>?>(true, "office", null) },
                { "Hub",                        Tuple.Create<bool, string, Func<string, string>?>(true, "hub", null) },
                { "Micro hub",                  Tuple.Create<bool, string, Func<string, string>?>(true, "micro_hub", null) },
                { "Fecha Incorporación",        Tuple.Create<bool, string, Func<string, string>?>(true, "incorporation_date", ExcelExtractorFilters.FilterDate) },
                { "Fecha Baja",                 Tuple.Create<bool, string, Func<string, string>?>(true, "leave_date", ExcelExtractorFilters.FilterDate) },
                { "Categoria",                  Tuple.Create<bool, string, Func<string, string>?>(true, "category", null) },
                { "Bussines Unit",              Tuple.Create<bool, string, Func<string, string>?>(true, "business_unit", null) },
                { "Division",                   Tuple.Create<bool, string, Func<string, string>?>(true, "division", null) },
                { "Department",                 Tuple.Create<bool, string, Func<string, string>?>(true, "department", null) },
                { "Servicio",                   Tuple.Create<bool, string, Func<string, string>?>(true, "service", null) },
                { "Service Team",               Tuple.Create<bool, string, Func<string, string>?>(true, "service_team", null) },
                { "% Asignación",               Tuple.Create<bool, string, Func<string, string>?>(true, "asignation", null) },
                { "Área Interna",               Tuple.Create<bool, string, Func<string, string>?>(true, "internal_area", null) },
                { "Sector",                     Tuple.Create<bool, string, Func<string, string>?>(true, "sector", null) },
                { "Horario",                    Tuple.Create<bool, string, Func<string, string>?>(true, "schedule", null) },
                { "Distribución de jornada",    Tuple.Create<bool, string, Func<string, string>?>(true, "workday_distribution", null) },
                { "Reducida",                   Tuple.Create<bool, string, Func<string, string>?>(true, "reduced_workday", null) },
                { "Dias Intensiva",             Tuple.Create<bool, string, Func<string, string>?>(true, "days_intensive", null) },
                { "Dias Teletrabajo",           Tuple.Create<bool, string, Func<string, string>?>(true, "days_remote", null) },
                { "Horario Teletrabajo",        Tuple.Create<bool, string, Func<string, string>?>(true, "remote_schedule", null) },
                { "Email",                      Tuple.Create<bool, string, Func<string, string>?>(true, "email", null) },
                { "Línea Tecnológica",          Tuple.Create<bool, string, Func<string, string>?>(true, "tecnologic_lane", null) },
                { "Tecnología",                 Tuple.Create<bool, string, Func<string, string>?>(true, "tecnology", null) },
                { "COE",                        Tuple.Create<bool, string, Func<string, string>?>(true, "coe", null) },
                { "Estudio",                    Tuple.Create<bool, string, Func<string, string>?>(true, "study", null) }

            };

            //NombreEnTabla<propiedad - importar> 


            return columnas;
        }

        private Dictionary<string, Tuple<bool, string, Func<string, string>?>> GetSchedulesColumns()
        {
            var columnasSchedule = new Dictionary<string, Tuple<bool, string, Func<string, string>?>>
            {
                {"numero_empleado", Tuple.Create<bool, string, Func<string, string>?>(true, "id_employee", null)},
                {"fecha",           Tuple.Create<bool, string, Func<string, string>?>(true, "date", ExcelExtractorFilters.FilterDate)},
                {"horas",           Tuple.Create<bool, string, Func<string, string>?>(true, "hours", null)}
            };

            return columnasSchedule;
        }

        private Dictionary<string, Tuple<bool, string, Func<string, string>?>> GetIncurredColumns()
        {
            var columnasIncurred = new Dictionary<string, Tuple<bool, string, Func<string, string>?>>
            {
                { "Numero Empleado", Tuple.Create<bool, string, Func<string, string>?>(true, "id_employee", null) },
                { "Id Task",         Tuple.Create<bool, string, Func<string, string>?>(true, "task_id", null) },
                { "Task Summary",    Tuple.Create<bool, string, Func<string, string>?>(true, "task_summary", ExcelExtractorFilters.FilterText) },
                { "Horas Incurridas",Tuple.Create<bool, string, Func<string, string>?>(true, "incurred_hours", null) },
                { "Fecha",           Tuple.Create<bool, string, Func<string, string>?>(true, "date", ExcelExtractorFilters.FilterDate) },
                { "Fecha Mes",       Tuple.Create<bool, string, Func<string, string>?>(true, "month_date", null) }

            };

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
                    await _repo.TruncateIncurred();
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
                string employeesPath = Path.Combine(_saveDirectory, "Headcount.xlsx");
                ExcelPackage excelEmployeesPackage = new ExcelPackage(employeesPath);
                ExcelWorksheet excelSheetEmployees = excelEmployeesPackage.Workbook.Worksheets["Detalle"];
                List<Employee> employeesData = _excelExtractor.GetDataAsList<Employee>(excelSheetEmployees, GetEmployeesColumns());
                File.Delete(employeesPath);

                // OBTENER EXCEL DE SCHEDULES.
                _sharePointDownloader.Download("Documentos%20compartidos/General/Data/horarios/octubre_2023.xlsx", _saveDirectory);
                string schedulesPath = Path.Combine(_saveDirectory, "octubre_2023.xlsx");
                ExcelPackage excelSchedulesPackage = new ExcelPackage(schedulesPath);
                ExcelWorksheet excelSheetSchedules = excelSchedulesPackage.Workbook.Worksheets["Horarios"];
                List<Schedule> schedulesData = _excelExtractor.GetDataAsList<Schedule>(excelSheetSchedules, GetSchedulesColumns());
                File.Delete(schedulesPath);

                // OBTENER EXCEL DE INCURRED
                
                _sharePointDownloader.Download("Documentos%20compartidos/General/Data/incurridos/Incurridos%20Periodo%20en%20curso.xlsx", _saveDirectory);
                string incurredsPath = Path.Combine(_saveDirectory, "Incurridos Periodo en curso.xlsx");
                ExcelPackage excelIncurredPackage = new ExcelPackage(incurredsPath);
                ExcelWorksheets worksheets = excelIncurredPackage.Workbook.Worksheets;
                ExcelWorksheet excelSheetIncurred = worksheets["Detalle Modificado"];
                List<Incurred> incurredsData = _excelExtractor.GetDataAsList<Incurred>(excelSheetIncurred, GetIncurredColumns());
                File.Delete(incurredsPath);
                

                try
                {
                    await _repo.InsertIntoTryEmployees(employeesData);
                    await _repo.InsertIntoTrySchedules(schedulesData);
                    await _repo.InsertIntoTryIncurred(incurredsData);

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
 