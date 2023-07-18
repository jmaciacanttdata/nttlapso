using NTTLapso.Models.General;

namespace NTTLapso.Models.Permissions
{
    public class PermissionDataResponse
    {
        public int? Id { get; set; }
        public string? Value { get; set; }
        public bool? Registration { get; set; }
        public bool? Read { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
    }
}
