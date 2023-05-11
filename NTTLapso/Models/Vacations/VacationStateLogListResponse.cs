using NTTLapso.Models.General;

namespace NTTLapso.Models.Vacations
{
    public class VacationStateLogListResponse
    {
        public bool IsSuccess { get; set; }
        public List<VacationStateLogDataResponse> Data { get; set; }
        public Error Error { get; set; }
    }
}
