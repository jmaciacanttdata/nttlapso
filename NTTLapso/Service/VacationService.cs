using NTTLapso.Models.General;
using NTTLapso.Models.Vacations;
using NTTLapso.Repository;
using System.Collections.Generic;

namespace NTTLapso.Service
{
    public class VacationService
    {
        public VacationRepository _repo;
        private IConfiguration _configuration;
        public VacationService(IConfiguration config) 
        {
            _configuration = config;    
            _repo = new VacationRepository(_configuration);
        }

        public async Task Create(CreateVacationRequest request)
        {
            await _repo.Create(request);
        }

         public async Task Edit(EditVacationRequest request)
        {
            await _repo.Edit(request);
        }

        // Delete vacation
        internal async Task Delete(int IdVacation)
        {
            await _repo.Delete(IdVacation);
        }
        internal async Task CreateLog(CreateLogRequest request)
        {
            await _repo.CreateLog(request);
        }

        public async Task VacationApproved(VacationApprovedRequest request)
        {
            await _repo.VacationApproved(request);
        }

        // Get vacation state log list.
        public async Task<List<VacationStateLogDataResponse>> VacationStateLogList(VacationStateLogListRequest? request)
        {
            return await _repo.VacationStateLogList(request);
        }
        public async Task<List<VacationData>> List(ListVacationRequest request) => await _repo.List(request);

        public async Task<List<VacationPendingsData>> Pendings(int Id) => await _repo.Pendings(Id);
        public async Task<List<PercentagePetitionDay>> GetPercentagePetitionUserPerDayMonthList(PercentagePerMonthRequest request)
        {
            return await _repo.GetPercentagePetitionUserPerDayMonthList(request);
        }
    }
}
