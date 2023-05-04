using NTTLapso.Models.RolPermission;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class RolPermissionService
    {
        private RolPermissionRepository _repo = new RolPermissionRepository();
        public RolPermissionService() { }

        // Get a list of rols with it's permissions.
        public async Task<List<RolPermissionDataResponse>> List(int? idRol)
        {
            return await _repo.List(idRol);
        }

        // Insert a new rol with permissions.
        public async Task Create(RolPermissionRequest request)
        {
            await _repo.Create(request);
        }

        // Update permissions from a rol.
        public async Task Edit(RolPermissionRequest request)
        {
            await _repo.Edit(request);
        }

        // Delete rol and it's permissions.
        public async Task Delete(int idRol)
        {
            await _repo.Delete(idRol);
        }
    }

}
