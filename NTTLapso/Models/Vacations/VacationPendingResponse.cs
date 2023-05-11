using NTTLapso.Models.General;

namespace NTTLapso.Models.Vacations
{
    public class VacationPendingResponse
    {
        public bool IsSuccess { get; set; }
        public List<VacationPendingsData>? Data { get; set; }
        public Error? Error { get; set; }
    }
}
