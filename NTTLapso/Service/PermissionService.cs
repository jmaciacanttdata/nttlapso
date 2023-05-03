using NTTLapso.Models.General;
using NTTLapso.Models.Permissions;
using NTTLapso.Models.PetitionStatus;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class PermissionService
    {
        private PermissionRepository _repo = new PermissionRepository();
        public PermissionService() { }

        public async Task<List<PermissionDataResponse>> List(PermissionDataResponse request)
        {
            return await _repo.List(request);
        }

        public async Task Create(CreatePermissionRequest request)
        {
            await _repo.Create(request);
        }

        public async Task Edit(EditPermissionRequest request)
        {
            await _repo.Edit(request);
        }

        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
    }
}
