using NTTLapso.Models.PetitionType;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class PetitionTypeService
    {
        private PetitionTypeRepository _repo = new PetitionTypeRepository();

        public PetitionTypeService() { }

        // Get petition type list.
        public async Task<List<PetitionTypeDataResponse>> List(PetitionTypeRequest request)
        {
            return await _repo.List(request);
        }

        // Create new petition type.
        public async Task Create(string value, bool selectable)
        {
            await _repo.Create(value, selectable);
        }

        // Edit a petition type.
        public async Task Edit(PetitionTypeRequest request)
        {
            await _repo.Edit(request);
        }

        // Delete a petition type
        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
    }
}
