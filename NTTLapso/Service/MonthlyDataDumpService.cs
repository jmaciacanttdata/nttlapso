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

            _sharePointDownloader = new SharePointDownloader(conf);

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

            double truncated_incurred = Math.Truncate(incurredUser.TotalIncurredHours);
            double incurred_decimal = incurredUser.TotalIncurredHours - truncated_incurred;

            float total_incurred = 0;

            if (incurred_decimal <= .4) total_incurred = (float)truncated_incurred;
            else if (1 - incurred_decimal <= .4) total_incurred = 1 + (float)truncated_incurred;
            else total_incurred = (float)truncated_incurred + .5f;

            incurredUser.HoursDiff = incurredUser.TotalHours - total_incurred;

            await _repo.CreateCalculated(incurredUser);
        }
        
        private List<Tuple<bool, string, string, Func<string, string>?>> GetEmployeesColumns()
        {
            var CreateTuple = Tuple.Create<bool, string, string, Func<string, string>?>;

            var columnas = new List<Tuple<bool, string, string, Func<string, string>?>>()
            {
                CreateTuple(true, "Numero Empleado", "id_employee", null),
                CreateTuple(true, "Persona", "name", null),
                CreateTuple(true, "Oficina", "office", null),
                CreateTuple(true, "Hub", "hub", null),
                CreateTuple(true, "Micro hub", "micro_hub", null),
                CreateTuple(true, "Fecha Incorporación", "incorporation_date", ExcelExtractorParsers.FilterDate),
                CreateTuple(true, "Fecha Baja", "leave_date", ExcelExtractorParsers.FilterDate),
                CreateTuple(true, "Categoria", "category", null),
                CreateTuple(true, "Bussines Unit", "business_unit", null),
                CreateTuple(true, "Division", "division", null),
                CreateTuple(true, "Department", "department", null),
                CreateTuple(true, "Servicio", "service", (data)=>{return data == "" ? "SIN SERVICIO" : data; }),
                CreateTuple(true, "Service Team", "service_team", (data)=>{return data=="" ? "EQUIPOS SIN NOMBRE" : data; }),
                CreateTuple(true, "% Asignación", "asignation", null),
                CreateTuple(true, "Área Interna", "internal_area", null),
                CreateTuple(true, "Sector", "sector", null),
                CreateTuple(true, "Horario", "schedule", null),
                CreateTuple(true, "Distribución de jornada", "workday_distribution", null),
                CreateTuple(true, "Reducida", "reduced_workday", null),
                CreateTuple(true, "Dias Intensiva", "days_intensive", null),
                CreateTuple(true, "Dias Teletrabajo", "days_remote", null),
                CreateTuple(true, "Horario Teletrabajo", "remote_schedule", null),
                CreateTuple(true, "Email", "email", null),
                CreateTuple(true, "Línea Tecnológica", "tecnologic_lane", null),
                CreateTuple(true, "Tecnología", "tecnology", null),
                CreateTuple(true, "COE", "coe", null),
                CreateTuple(true, "Estudio", "study", null)

            };

            //NombreEnTabla<propiedad - importar> 


            return columnas;
        }

        private List<Tuple<bool, string, string, Func<string, string>?>> GetSchedulesColumns()
        {
            var CreateTuple = Tuple.Create<bool, string, string, Func<string, string>?>;

            var columnasSchedule = new List<Tuple<bool, string, string, Func<string, string>?>>
            {
                CreateTuple(true, "numero_empleado", "id_employee", null),
                CreateTuple(true, "fecha", "date", ExcelExtractorParsers.FilterDate),
                CreateTuple(true, "horas", "hours", null)
            };

            return columnasSchedule;
        }

        private List<Tuple<bool, string, string, Func<string, string>?>> GetIncurredColumns()
        {
            var CreateTuple = Tuple.Create<bool, string, string, Func<string, string>?>;

            var columnasIncurred = new List<Tuple<bool, string, string, Func<string, string>?>>
            {
                CreateTuple(true, "Numero Empleado",        "id_employee",              null),
                CreateTuple(true, "Nombre Servicio",        "service_name",             (data)=>{return data=="" ? "SEVICIOS SIN NOMBRE" : data; }),
                CreateTuple(true, "Service Team",           "service_team",             (data)=>{return data=="" ? "EQUIPOS SIN NOMBRE" : data; }),
                CreateTuple(true, "Pkey Jira",              "pkey_jira",                null),
                CreateTuple(true, "Componente",             "component",                null),
                CreateTuple(true, "Agrupación",             "grouping",                 null),
                CreateTuple(true, "Service Line",           "service_line",             null),
                CreateTuple(true, "Tipo Task",              "task_type",                null),
                CreateTuple(true, "Facturable a cliente",   "billable_to_customer",     null),
                CreateTuple(true, "Id Task",                "task_id",                  null),
                CreateTuple(true, "Task Summary",           "task_summary",             ExcelExtractorParsers.FilterText),
                CreateTuple(true, "Estado Task",            "task_state",               null),
                CreateTuple(true, "Origen Task",            "task_origin",              null),
                CreateTuple(true, "Estimación Interna",     "intern_estimation",        null),
                CreateTuple(true, "Estimacion Agile",       "agile_estimation",         null),
                CreateTuple(true, "Unidad Estimacion",      "estimation_unit",          null),
                CreateTuple(true, "Tipo Sub Task",          "subtask_type",             null),
                CreateTuple(true, "Typology",               "typology",                 null),
                CreateTuple(true, "Id Sub Task",            "subtask_id",               null),
                CreateTuple(true, "Sub Task Summary",        "subtask_summary",          ExcelExtractorParsers.FilterText),
                CreateTuple(true, "Estado Sub Task",        "subtask_state",            null),
                CreateTuple(true, "Origen Sub Task",        "subtask_origin",           null),
                CreateTuple(true, "Comentario Incurrido",   "incurred_comment",         ExcelExtractorParsers.FilterText),
                CreateTuple(true, "Estimacion Sub Task",    "subtask_estimation",       null),
                CreateTuple(true, "Horas Incurridas",       "incurred_hours",           null),
                CreateTuple(true, "Fecha",                  "date",                     ExcelExtractorParsers.FilterDate)
            };

            return columnasIncurred;
        }

        private List<T> DownloadAndExtractExcelData<T>(string pathToFileFromServer, string downloadFilename, string worksheetName, Func<List<Tuple<bool, string, string, Func<string, string>?>>> dictionaryFunc) 
        {
            _sharePointDownloader.Download(pathToFileFromServer, _saveDirectory);
            string employeesPath = Path.Combine(_saveDirectory, downloadFilename);
            ExcelPackage package = new ExcelPackage(employeesPath);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[worksheetName];
            List<T> data = _excelExtractor.GetDataAsList<T>(worksheet, dictionaryFunc());
            File.Delete(employeesPath);
            return data;
        }

        private async Task<SimpleResponse> EmployeeExists(string? userId)
        {
            LogBuilder log = new LogBuilder();
            var resp = new SimpleResponse();

            try
            {
                log.LogIf("Comprobando si el empleado " + userId + " existe.");
                resp.Completed = await _repo.EmployeeExists(userId);
                if (!resp.Completed)
                {
                    log.LogErr("El empleado solicitado no existe.");
                    resp.StatusCode = 400;
                }
                else
                {
                    log.LogOk("Empleado encontrado.");
                }
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.StatusCode = 500;
                
            }

            resp.Log = log;

            return resp;
        }

        public async Task<SimpleResponse> GetLeaderRemainingHours(string? leader_id, string? employee_id, string? service)
        {
            LogBuilder log = new LogBuilder();
            var resp = new LeaderIncurredHoursResponse();

            try
            {
                var respExists = await EmployeeExists(leader_id);
                if (leader_id != null && !respExists.Completed)
                {
                    log.Append(respExists.Log);
                    resp.Completed = respExists.Completed;
                    resp.StatusCode = 400;
                    resp.Log = log;
                    return resp;
                }

                respExists = await EmployeeExists(employee_id);
                if (employee_id != null && !respExists.Completed)
                {
                    log.Append(respExists.Log);
                    resp.Completed = respExists.Completed;
                    resp.StatusCode = 400;
                    resp.Log = log;
                    return resp;
                }

                log.LogIf("Obteniendo listado de empleados por equipos...");
                resp.DataList = await _repo.GetLeaderRemainingHours(leader_id, employee_id, service);
                log.LogOk("Lista de empleados por equipos obtenida.");

                resp.DataList.ForEach(x => { resp.TotalRemainingHours += x.remaining_hours > 0 ? x.remaining_hours : 0; });

                resp.Completed = true;
                resp.StatusCode = 200;
                resp.Log = log;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.StatusCode = 500;
                resp.Log = log;
            }

            return resp;
        } 

        public async Task<SimpleResponse> GetEmployeeRemainingHours(string month, string year, string? userId = null)
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
                    resp.Log = log;
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

            resp.Log = log;
            return resp;
        }

        public async Task<SimpleResponse> GetTotalIncurredHours(string month, string year, string? userId)
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
                    resp.Log = log;
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

            resp.Log = log;
            return resp;
        }

        public async Task<SimpleResponse> GetIncurredHours(string month, string year, string? userId)
        {
            LogBuilder log = new LogBuilder();
            var resp = new IncurredHoursByDateResponse();

            try
            {
                var respExists = await EmployeeExists(userId);
                if (userId != null && !respExists.Completed)
                {
                    log.Append(respExists.Log);
                    resp.Completed = respExists.Completed;
                    resp.StatusCode = 400;
                    resp.Log = log;
                }

                log.LogIf("Obteniendo lista de empleados con sus horas incurridas en el último mes...");
                resp.IncurredList = await _repo.GetIncurredHoursByDate(month, year, userId);

                if (resp.IncurredList.Count == 0)
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

            resp.Log = log;
            return resp;
        }

        public async Task<SimpleResponse> GetConsolidatedEmployees() 
        {
            var resp = new ConsolidationResponse();
            LogBuilder log = new LogBuilder();

            try
            {
                log.LogIf("Obteniendo empleados consolidados...");
                resp.ConsolidatedEmployees = await _repo.GetConsolidatedEmployees();
                log.LogOk("Empleados consolidados obtenidos satisfactoriamente.");

                resp.Completed = true;
                resp.Log = log;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = true;
                resp.StatusCode = 500;
                resp.Log = log;
            }

            return resp;
        }

        public async Task<NumConsolidationResponse> CreateConsolidation()
        {
            LogBuilder log = new LogBuilder();
            var resp = new NumConsolidationResponse();

            try
            {
                log.LogIf("Creando tabla de consolidacion...");
                int consolidatedEntries = await _repo.CreateConsolidation();
                log.LogOk("Tabla de consolidacion creada correctamente. "+ consolidatedEntries + " empleados consolidados.");

                resp.Completed = true;
                resp.NumConsolidate = consolidatedEntries;
                resp.Log = log;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.StatusCode = 500;
                resp.NumConsolidate = 0;
                resp.Log = log;
            }

            return resp;
        }

        public async Task<SimpleResponse> DumpEmployeesIntoUsers()
        {
            LogBuilder log = new LogBuilder();
            var resp = new SimpleResponse();

            try
            {
                log.LogIf("Volcando empleados nuevos en usuarios...");
                await _repo.DumpEmployeesIntoUsers();
                log.LogOk("Se han volcado los empleados en usuarios correctamente.");

                resp.Completed = true;
                resp.StatusCode = 200;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.StatusCode = 500;
            }

            resp.Log = log;

            return resp;
        }

        public async Task<SimpleResponse> CreateLeaderRemainingHours()
        {
            LogBuilder log = new LogBuilder();
            var resp = new SimpleResponse();

            try
            {
                log.LogIf("Creando tabla de horas incurridas por empleados supervisados por lideres...");
                await _repo.CreateLeaderRemainingHours();
                log.LogOk("Tabla creada correctamente.");
                resp.Completed = true;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.StatusCode = 500;
            }

            resp.Log = log;
            return resp;
        }

        public async Task<SimpleResponse> CalculateMonthlyHours()
        {
            var resp = new SimpleResponse();
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
                    resp.Log = log;
                }
                else
                {
                    log.LogErr("No hay empleados en la base de datos.");
                    resp.Completed = false;
                    resp.Log = log;
                }
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                resp.Completed = false;
                resp.Log = log;
                resp.StatusCode = 500;
            }

            return resp;
        }
    
        public async Task<NumConsolidationResponse> LoadDataFromExcels()
        {
            var resp = new NumConsolidationResponse();
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
                List<Schedule> schedulesData = DownloadAndExtractExcelData<Schedule>("Documentos%20compartidos/General/Data/Horarios/noviembre_2023.xlsx", "noviembre_2023.xlsx", "Employees Schedules", GetSchedulesColumns);
                log.LogOk("Datos de horarios del excel leídos correctamente.  ");

                log.LogIf("Leyendo datos de incurridos del excel...");
                List<Incurred> incurredsData = DownloadAndExtractExcelData<Incurred>("Documentos%20compartidos/General/Data/Incurridos/Incurridos%20Periodo%20en%20curso.xlsx", "Incurridos Periodo en curso.xlsx", "Detalle", GetIncurredColumns);
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
                resp.Log = log;
                return resp;
            }
            catch (Exception e)
            {
                log.LogErr(e.Message);

                resp.Completed = false;
                resp.StatusCode = 500;
                resp.Log = log;
                return resp;
            }
        }
    }
}
 