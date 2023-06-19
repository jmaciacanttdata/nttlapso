using NTTLapso.Models.General;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class UserScheduleService
    {
        private UserScheduleRepository _repo;
        private IConfiguration _configuration;
        public UserScheduleService(IConfiguration config) 
        {
            _configuration = config;
            _repo = new UserScheduleRepository(_configuration);
        }

        public async Task<List<IdValue>> List(IdValue request)
        {
            return await _repo.List(request);
        }

        public async Task Create(string value)
        {
            await _repo.Create(value);
        }

        public async Task Edit(IdValue request)
        {
            await _repo.Edit(request);
        }

        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
    }
}
