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

        public async Task Edit(int id)
        {
            //await _repo.Edit(id);
        }

        // Get vacation state log list.
        public async Task<List<VacationStateLogDataResponse>> VacationStateLogList(VacationStateLogListRequest? request)
        {
            return await _repo.VacationStateLogList(request);
        }

        // Delete vacation
        internal async Task Delete(int IdVacation)
        {
            await _repo.Delete(IdVacation);
        }
    }
}
