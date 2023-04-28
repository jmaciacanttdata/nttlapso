using NTTLapso.Models.RolPermission;

namespace NTTLapso.Models.RolPermission
{
    public class RolPermissionDataResponse
    {
        public int IdRol { get; set; }

        public string Value { get; set; }

        public List<PermissionDataResponse> PermissionDataList { get; set; } = new List<PermissionDataResponse>();
    }
}
