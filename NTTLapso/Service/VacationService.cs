using NTTLapso.Models.Vacations;
using NTTLapso.Repository;
using System.Collections.Generic;

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
        public async Task<List<VacationData>> List(ListVacationRequest request) => await _repo.List(request);
    }
}
