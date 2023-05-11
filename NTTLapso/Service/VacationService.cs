using NTTLapso.Models.General;
using NTTLapso.Models.Vacations;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class VacationService
    {
        public VacationRepository _repo = new VacationRepository();

        public VacationService() { }

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

        public async Task VacationApproved(VacationApprovedRequest request)
        {
            await _repo.VacationApproved(request);
        }

        // Get vacation state log list.
        public async Task<List<VacationStateLogDataResponse>> VacationStateLogList(VacationStateLogListRequest? request)
        {
            return await _repo.VacationStateLogList(request);
        }
    }
}
