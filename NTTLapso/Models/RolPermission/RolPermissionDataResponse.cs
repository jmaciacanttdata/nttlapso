

using NTTLapso.Models.Permissions;

namespace NTTLapso.Models.RolPermission
{
    public class RolPermissionDataResponse
    {
        public int IdRol { get; set; }

        public string Value { get; set; }

        public List<PermissionDataResponse> PermissionList { get; set; } = new List<PermissionDataResponse>();
    }
}
