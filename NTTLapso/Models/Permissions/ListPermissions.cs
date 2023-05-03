using NTTLapso.Models.General;

namespace NTTLapso.Models.Permissions
{
    public class ListPermissions
    {
        public bool IsSuccess { get; set; }
        public List<PermissionDataResponse> Data { get; set; }
        public Error Error { get; set; }

    }
}
