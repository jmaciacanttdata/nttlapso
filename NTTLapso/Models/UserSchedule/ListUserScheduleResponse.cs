using NTTLapso.Models.General;

namespace NTTLapso.Models.UserSchedule
{
    public class ListUserScheduleResponse
    {
        public bool IsSuccess { get; set; }
        public List<IdValue> Data { get; set; }
        public Error Error { get; set; }
    }
}
