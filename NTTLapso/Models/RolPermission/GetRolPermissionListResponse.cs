using NTTLapso.Models.General;

namespace NTTLapso.Models.RolPermission
{
    public class GetRolPermissionListResponse
    {
        public List<RolPermissionDataResponse> Data { get; set; } = new List<RolPermissionDataResponse>();

        public ErrorResponse Error { get; set; } = new ErrorResponse();
    }
}
