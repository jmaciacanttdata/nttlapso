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
            _repo = new MonthlyDataDumpRepository(conf);
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
  
        private async Task DoUserCalc(Employee employee)
        {
            MonthlyIncurredHours incurredUser = new MonthlyIncurredHours();
            incurredUser.IdEmployee = employee.id_employee;
            incurredUser.Year = ((DateTime.Now.Month == 1) ? DateTime.Now.Year - 1 : DateTime.Now.Year).ToString();
            incurredUser.Month = ((DateTime.Now.Month == 1) ? 12 : DateTime.Now.Month - 1).ToString();
            incurredUser.TotalHours = await _repo.GetTotalHours(employee.id_employee);
            incurredUser.TotalIncurredHours = await _repo.GetIncurred(employee.id_employee, incurredUser.Month);

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
                { "Service Team",               Tuple.Create<bool, string, Func<string, string>?>(true, "service_team", (data)=>{return data=="" ? "EQUIPOS SIN NOMBRE" : data; }) },
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
                { "Service Team",    Tuple.Create<bool, string, Func<string, string>?>(true, "service_team", (data)=>{return data=="" ? "EQUIPOS SIN NOMBRE" : data; }) },
                { "Id Task",         Tuple.Create<bool, string, Func<string, string>?>(true, "task_id", null) },
                { "Task Summary",    Tuple.Create<bool, string, Func<string, string>?>(true, "task_summary", ExcelExtractorFilters.FilterText) },
                { "Horas Incurridas",Tuple.Create<bool, string, Func<string, string>?>(true, "incurred_hours", null) },
                { "Fecha",           Tuple.Create<bool, string, Func<string, string>?>(true, "date", ExcelExtractorFilters.FilterDate) },
                { "Fecha Mes",       Tuple.Create<bool, string, Func<string, string>?>(true, "month_date", null) }

            };

            return columnasIncurred;
        }

        private List<T> DownloadAndExtractExcelData<T>(string pathToFileFromServer, string downloadFilename, string worksheetName, Func<Dictionary<string, Tuple<bool, string, Func<string, string>?>>> dictionaryFunc) 
        {
            _sharePointDownloader.Download(pathToFileFromServer, _saveDirectory);
            string employeesPath = Path.Combine(_saveDirectory, downloadFilename);
            ExcelPackage package = new ExcelPackage(employeesPath);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetName];
            List<T> data = _excelExtractor.GetDataAsList<T>(worksheet, dictionaryFunc());
            File.Delete(employeesPath);
            return data;
        }

        private async Task<EmployeeExistsResponse> EmployeeExists(string? userId)
        {
            LogBuilder log = new LogBuilder();
            var resp = new EmployeeExistsResponse();

            log.LogIf("Comprobando si el empleado " + userId + " existe.");
            resp.Completed = await _repo.EmployeeExists(userId);
            if (!resp.Completed)
            {
                log.LogErr("El empleado solicitado no existe.");
            }
            else
            {
                log.LogOk("Empleado encontrado.");
            }
            resp.Log = log.Message;
            

            return resp;
        }

        public async Task<IncurredHoursByServiceResponse> GetIncurredHoursByService(string? leader_id, string? employee_id, string? service)
        {
            LogBuilder log = new LogBuilder();
            var resp = new IncurredHoursByServiceResponse();

            try
            {
                var respExists = await EmployeeExists(leader_id);
                if (leader_id != null && !respExists.Completed)
                {
                    log.Append(respExists.Log);
                    resp.Completed = respExists.Completed;
                    resp.StatusCode = 400;
                    resp.Log = log.Message;
                    return resp;
                }

                respExists = await EmployeeExists(employee_id);
                if (employee_id != null && !respExists.Completed)
                {
                    log.Append(respExists.Log);
                    resp.Completed = respExists.Completed;
                    resp.StatusCode = 400;
                    resp.Log = log.Message;
                    return resp;
                }

                log.LogIf("Obteniendo listado de empleados por equipos...");
                resp.DataList = await _repo.GetIncurredHoursByService(leader_id, employee_id, service);
                log.LogOk("Lista de empleados por equipos obtenida.");

                resp.DataList.ForEach(x => { resp.TotalRemainingHours += x.remaining_hours > 0 ? x.remaining_hours : 0; });

                resp.Completed = true;
                resp.StatusCode = 200;
                resp.Log = log.Message;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.StatusCode = 500;
                resp.Log = log.Message;
            }

            return resp;
        } 

        public async Task<EmployeeRemainingHoursResponse> GetEmployeeRemainingHours(string month, string year, string? userId = null)
        {
            LogBuilder log = new LogBuilder();
            var resp = new EmployeeRemainingHoursResponse();
            try
            {
                var respExists = await EmployeeExists(userId);
                if(userId != null && !respExists.Completed)
                {
                    log.Append(respExists.Log);
                    resp.Completed = respExists.Completed;
                    resp.StatusCode = 400;
                    resp.Log = log.Message;
                }
                
                log.LogIf("Obteniendo lista de horas por incurrir de los empleados...");
                resp.EmployeesList = await _repo.GetRemainingIncurredHours(month, year, userId);


                if (resp.EmployeesList.Count == 0)
                {
                    resp.Completed = true;
                    resp.StatusCode = 200;
                    log.LogKo("Lista obtenida satisfactoriamente pero no hay empleados.");
                }
                else
                {
                    resp.Completed = true;
                    resp.StatusCode = 200;
                    log.LogOk("Lista obtenida satisfactoriamente.");
                }

            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.StatusCode = 500;
            }

            resp.Log = log.Message;
            return resp;
        }

        public async Task<EmployeeMonthlyIncurredHoursResponse> GetTotalIncurredHours(string month, string year, string? userId)
        {
            LogBuilder log = new LogBuilder();
            var resp = new EmployeeMonthlyIncurredHoursResponse();

            try
            {
                var respExists = await EmployeeExists(userId);
                if (userId != null && !respExists.Completed)
                {
                    log.Append(respExists.Log);
                    resp.Completed = respExists.Completed;
                    resp.StatusCode = 400;
                    resp.Log = log.Message;
                }

                log.LogIf("Obteniendo lista de empleados con sus horas incurridas en el último mes...");
                resp.EmployeesList = await _repo.GetTotalIncurredHoursByDate(month, year, userId);

                if(resp.EmployeesList.Count == 0)
                {
                    log.LogKo("La peticion se resolivio correctamente pero no hay empleados.");
                    resp.Completed = true;
                    resp.StatusCode = 200;
                }
                else
                {
                    log.LogOk("Lista de empleados obtenida satisfactoriamente.");
                    resp.Completed = true;
                    resp.StatusCode = 200;
                }  
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.StatusCode = 500;
            }

            resp.Log = log.Message;
            return resp;
        }

        public async Task<ConsolidationResponse> GetConsolidatedEmployees() 
        {
            ConsolidationResponse resp = new ConsolidationResponse();
            LogBuilder log = new LogBuilder();

            try
            {
                log.LogIf("Obteniendo empleados consolidados...");
                resp.ConsolidatedEmployees = await _repo.GetConsolidatedEmployees();
                log.LogOk("Empleados consolidados obtenidos satisfactoriamente.");

                resp.Completed = true;
                resp.Log = log.Message;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = true;
                resp.Log = log.Message;
            }

            return resp;
        }

        public async Task<DataDumpResponse> CreateConsolidation()
        {
            LogBuilder log = new LogBuilder();
            DataDumpResponse resp = new DataDumpResponse();

            try
            {
                log.LogIf("Creando tabla de consolidacion...");
                int consolidatedEntries = await _repo.CreateConsolidation();
                log.LogOk("Tabla de consolidacion creada correctamente. "+ consolidatedEntries + " empleados consolidados.");

                resp.Completed = true;
                resp.NumConsolidate = consolidatedEntries;
                resp.Log = log.Message;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.NumConsolidate = 0;
                resp.Log = log.Message;
            }

            return resp;
        }

        public async Task<CalculateHoursResponse> CalculateMonthlyHours()
        {
            CalculateHoursResponse resp = new CalculateHoursResponse();
            List<Employee> Users = new List<Employee>();
            LogBuilder log = new LogBuilder();

            log.LogIf("Iniciado proceso de cálculo de horas mensuales...");

            try
            {
                log.LogIf("Obteniendo empleados de la base de datos...");
                Users = await _repo.GetUsers();
                if (Users.Count() > 0)
                {
                    log.LogOk("Empleados obtenidos correctamente.");
                    log.LogIf("Haciendo calculo de horas de los empleados e insertandolos en la tabla histórica...");
                    foreach (Employee user in Users)
                    {
                        await DoUserCalc(user);
                    }
                    log.LogOk("Proceso de cálculo de horas mensuales realizado correctamente.");

                    resp.Completed = true;
                    resp.Log = log.Message;
                }
                else
                {
                    log.LogErr("No hay empleados en la base de datos.");
                    resp.Completed = false;
                    resp.Log = log.Message;
                }
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.Log = log.Message;
            }

            return resp;
        }
    
        public async Task<DataDumpResponse> LoadDataFromExcels()
        {
            DataDumpResponse resp = new DataDumpResponse();
            LogBuilder log = new LogBuilder();
            
            try
            {
                log.LogIf("Cargando datos de los excels...");

                // TRUNCADO DE DATOS.
                log.LogIf("Truncando tabla incurred...");
                await _repo.TruncateIncurred();
                log.LogOk("Tabla incurred truncada correctamente.");

                log.LogIf("Truncando tabla schedules...");
                await _repo.TruncateSchedules();
                log.LogOk("Tabla schedules truncada correctamente.");
                
                log.LogIf("Truncando tabla employees...");
                await _repo.TruncateEmployees();
                log.LogOk("Tabla employees truncada correctamente.");

                // OBTENER DATOS DE LOS EXCELS.
                log.LogIf("Leyendo datos de empleados del excel...");
                List<Employee> employeesData = DownloadAndExtractExcelData<Employee>("Documentos%20compartidos/General/Data/Headcount.xlsx", "Headcount.xlsx", "Detalle", GetEmployeesColumns);
                log.LogOk("Datos de empleados del excel leídos correctamente.  ");

                log.LogIf("Leyendo datos de horarios del excel...");
                List<Schedule> schedulesData = DownloadAndExtractExcelData<Schedule>("Documentos%20compartidos/General/Data/horarios/octubre_2023.xlsx", "octubre_2023.xlsx", "Employees Schedules", GetSchedulesColumns);
                log.LogOk("Datos de horarios del excel leídos correctamente.  ");

                log.LogIf("Leyendo datos de incurridos del excel...");
                List<Incurred> incurredsData = DownloadAndExtractExcelData<Incurred>("Documentos%20compartidos/General/Data/incurridos/Incurridos%20Periodo%20en%20curso.xlsx", "Incurridos Periodo en curso.xlsx", "Detalle", GetIncurredColumns);
                log.LogOk("Datos de incurridos del excel leídos correctamente.  ");

                // INSERCION DE DATOS EN TABLAS.

                log.LogIf("Insertando empleados del excel en la base de datos...");
                await _repo.InsertEmployees(employeesData);
                log.LogOk("Empleados insertados correctamente.");

                log.LogIf("Insertando horarios del excel en la base de datos...");
                await _repo.InsertSchedules(schedulesData);
                log.LogOk("Horarios insertados correctamente.");

                log.LogIf("Insertando incurridos del excel en la base de datos...");
                await _repo.InsertIncurreds(incurredsData);
                log.LogOk("Incurridos insertados correctamente.");


                log.LogOk("Se ha completado el volcado de datos.");

                resp.Completed = true;
                resp.Log = log.Message;
                return resp;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);

                resp.Completed = false;
                resp.Log = log.Message;
                return resp;
            }
        }
    }
}
 