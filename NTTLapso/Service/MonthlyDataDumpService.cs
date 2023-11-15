using NTTLapso.Models.DataDump;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class MonthlyDataDumpService
    {
        private MonthlyDataDumpRepository _repo;
        private IConfiguration _conf;

        public MonthlyDataDumpService(IConfiguration conf)
        {
            _conf = conf;
            _repo = new MonthlyDataDumpRepository(conf);
        }

        private async Task DoUserCalc(string userId)
        {
            MonthlyIncurredHours incurredUser = new MonthlyIncurredHours();
            incurredUser.NumEmpleado = userId;
            incurredUser.HorasIncurrir = await _repo.GetTotalHoras(userId);
            incurredUser.HorasIncurridas = await _repo.GetIncurred(userId);
            incurredUser.Anyo = (DateTime.Now.Month == 1) ? DateTime.Now.Year - 1 : DateTime.Now.Year;
            incurredUser.Mes = (DateTime.Now.Month == 1) ? 12 : DateTime.Now.Month - 1;
            await _repo.CreateCalculated(incurredUser);
        }

        public async Task<CustomResponseCharge> DumpData(string? userId = null)
        {
            CustomResponseCharge result = new CustomResponseCharge();
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
    }
}
 