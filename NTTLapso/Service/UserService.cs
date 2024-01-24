using NTTLapso.Models.DataDump;
using NTTLapso.Models.Permissions;
using NTTLapso.Models.Process.UserCharge;
using NTTLapso.Models.Users;
using NTTLapso.Repository;
using NTTLapso.Tools;

namespace NTTLapso.Service
{
    public class UserService
    {
        private UserRepository _repo;
        private MonthlyDataDumpRepository _repoData;
        private IConfiguration _configuration;

        public UserService(IConfiguration config)
        {
            _configuration = config;
            _repo = new UserRepository(_configuration);
            _repoData = new MonthlyDataDumpRepository(_configuration);
        }

        public async Task<List<UserDataResponse>> List(UserListRequest request)
        {
            return await _repo.List(request);
        }

        private async Task<SimpleResponse> EmployeeExists(string? userId)
        {
            LogBuilder log = new LogBuilder();
            var resp = new SimpleResponse();

            try
            {
                log.LogIf("Comprobando si el empleado " + userId + " existe.");
                resp.Completed = await _repoData.EmployeeExists(userId);
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
        public async Task<UserRankResponse> GetUserRank(UserListRequest user)
        {
            LogBuilder log = new LogBuilder();
            UserRankResponse response = new UserRankResponse();

            try
            {
                SimpleResponse responseExists = await this.EmployeeExists(user.Id.ToString());
                if (user.Id != null && !responseExists.Completed)
                {
                    log.Append(responseExists.Log);
                    response.Completed = responseExists.Completed;
                    response.StatusCode = 400;
                    response.Log = log;
                    return response;
                }

                log.LogIf("Obteniendo obteniendo el rango del supervisor con id " + user.Id + "...");
                response.DataList = await _repo.GetUserRank(user);
                log.LogIf("Rango obtenido correctamente");

                response.Completed = true;
                response.StatusCode = 200;
                response.Log = log;

            }
            catch (Exception e)
            {
                log.LogErr(e.Message);
                response.Completed = false;
                response.StatusCode = 500;
                response.Log = log;
            }
            return response;
        }
        public async Task<NewUserChargeRequest> Create(CreateUserRequest request)
        {
            return await _repo.Create(request);
        }
        public async Task Edit(EditUserRequest request)
        {
            await _repo.Edit(request);
        }
        public async Task ChangeUserState(ChangeUserStateRequest request)
        {
            await _repo.ChangeUserState(request);
        }
        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
        internal async Task SetUserTeam(UserTeamRequest request)
        {
            await _repo.SetUserTeam(request);
        }
    }
}
