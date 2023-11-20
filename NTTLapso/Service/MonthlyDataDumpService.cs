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

        private Dictionary<string, Tuple<bool, Func<string, string>?>> GetEmployeesColumns()
        {

            var columnas = new Dictionary<string, Tuple<bool, Func<string, string>?>>
            {
                { "Numero Empleado", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Persona", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Oficina", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Hub", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Micro hub", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Fecha Incorporación", Tuple.Create<bool, Func<string, string>?>(true, ExcelExtractorFilters.FilterDate) },
                { "Fecha Baja", Tuple.Create<bool, Func<string, string>?>(true, ExcelExtractorFilters.FilterDate) },
                { "Categoria", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Bussines Unit", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Division", Tuple.Create<bool, Func<string, string>?>(true, null)},
                { "Department", Tuple.Create<bool, Func<string, string>?>(true, null)},
                { "Servicio", Tuple.Create<bool, Func<string, string>?>(true, null)},
                { "Service Team", Tuple.Create<bool, Func<string, string>?>(true, null)},
                { "% Asignación", Tuple.Create<bool, Func<string, string>?>(true, null)},
                { "Área Interna", Tuple.Create<bool, Func<string, string>?>(true, null)},
                { "Sector", Tuple.Create<bool, Func<string, string>?>(true, null)},
                { "Horario", Tuple.Create<bool, Func<string, string>?>(true, null)},
                { "Distribución de jornada", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Reducida", Tuple.Create<bool, Func<string, string>?>(true, null)},
                { "Dias Intensiva", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Dias Teletrabajo", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Horario Teletrabajo", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Email", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Línea Tecnológica", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Tecnología", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "COE", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Estudio", Tuple.Create<bool, Func<string, string>?>(true, null) }

            };

            //NombreEnTabla<propiedad - importar> 


            return columnas;
        }

        private Dictionary<string, Tuple<bool, Func<string, string>?>> GetSchedulesColumns()
        {
            var columnasSchedule = new Dictionary<string, Tuple<bool, Func<string, string>?>>
            {
                {"numero_empleado", Tuple.Create<bool, Func<string, string>?>(true, null)},
                {"fecha", Tuple.Create<bool, Func<string, string>?>(true, ExcelExtractorFilters.FilterDate)},
                {"horas", Tuple.Create<bool, Func<string, string>?>(true, null)}
            };

            return columnasSchedule;
        }

        private Dictionary<string, Tuple<bool, Func<string, string>?>> GetIncurredColumns()
        {
            var columnasIncurred = new Dictionary<string, Tuple<bool, Func<string, string>?>>

            {
                { "Numero Empleado", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Nombre Persona", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Situación actual persona", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Pkey Jira", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Componente", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Agrupación", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Service Line", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Tipo Task", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Facturable a cliente", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Id Task", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Task Summary", Tuple.Create<bool, Func<string, string>?>(true, ExcelExtractorFilters.FilterText) },
                { "Estado Task", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Origen Task", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Estimación Interna", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Estimacion Agile", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Unidad Estimacion", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Tipo Sub Task", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Typology", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Id Sub Task", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Sub Task Summary", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Estado Sub Task", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Origen Sub Task", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Comentario Incurrido", Tuple.Create<bool, Func<string, string>?>(true, ExcelExtractorFilters.FilterText) },
                { "Estimacion Sub Task", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Horas Incurridas", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "ETC", Tuple.Create<bool, Func<string, string>?>(true, null) },
                { "Fecha", Tuple.Create<bool, Func<string, string>?>(true, ExcelExtractorFilters.FilterDate) },
                { "Fecha Mes", Tuple.Create<bool, Func<string, string>?>(true, null) }

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
                string employeesInsert = _excelExtractor.GetDataAsInsertQuery(excelSheetEmployees, GetEmployeesColumns());

                // OBTENER EXCEL DE SCHEDULES.
                _sharePointDownloader.Download("Documentos%20compartidos/General/Data/horarios/octubre_2023.xlsx", _saveDirectory);
                ExcelPackage excelSchedulesPackage = new ExcelPackage(Path.Combine(_saveDirectory, "octubre_2023.xlsx"));
                ExcelWorksheet excelSheetSchedules = excelSchedulesPackage.Workbook.Worksheets["Horarios"];
                string schedulesInsert = _excelExtractor.GetDataAsInsertQuery(excelSheetSchedules, GetSchedulesColumns());

                // OBTENER EXCEL DE INCURRED
                /*
                _sharePointDownloader.Download("Documentos%20compartidos/General/Data/incurridos/Incurridos%20Periodo%20en%20curso.xlsx", _saveDirectory);
                ExcelPackage excelIncurredPackage = new ExcelPackage(Path.Combine(_saveDirectory, "Incurridos Periodo en curso.xlsx"));
                ExcelWorksheets worksheets = excelIncurredPackage.Workbook.Worksheets;
                ExcelWorksheet excelSheetIncurred = worksheets["Detalle Modificado"];
                string incurredsInsert = _excelExtractor.GetDataAsInsertQuery(excelSheetIncurred, GetIncurredColumns());
                */

                try
                {
                    await _repo.InsertEmployees(employeesInsert);
                    await _repo.InsertSchedules(schedulesInsert);
                    //await _repo.InsertIncurred(incurredsInsert);

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
 