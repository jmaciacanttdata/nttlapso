using NTTLapso.Models.General;

namespace NTTLapso.Models.Permissions
{
    public class PermissionResponse
    {
        public bool IsSuccess { get; set; }
        public Error Error { get; set; }
    }
}
