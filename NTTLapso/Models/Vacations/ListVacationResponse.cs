using NTTLapso.Models.General;

namespace NTTLapso.Models.Vacations
{
    public class ListVacationResponse
    {
        public bool IsSuccess { get; set; }
        public List<VacationData> Data { get; set; }
        public Error? Error { get; set; }
    }
}
