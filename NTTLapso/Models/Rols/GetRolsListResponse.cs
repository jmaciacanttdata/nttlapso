using NTTLapso.Models.General;

namespace NTTLapso.Models.Rols
{
    public class GetRolsListResponse
    {
        public List<IdValue>? RolsList { get; set; }

        public string? ErrorMessage { get; set; }

        public string? ErrorType { get; set; }
    }
}
