using NTTLapso.Models.General;

namespace NTTLapso.Models.RolPermission
{
    public class RolPermissionListResponse
    {
        public bool IsSuccess { get; set; }
        public List<RolPermissionDataResponse> Data { get; set; }
        public Error? Error { get; set; }
    }
}
