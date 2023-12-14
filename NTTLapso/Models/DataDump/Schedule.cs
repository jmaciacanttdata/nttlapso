
namespace NTTLapso.Models.DataDump
{ 
    public class Schedule
    {
        public string id_employee { get; set; }
        public string date {  get; set; }
        public string hours { get; set; }

    }

    public class ScheduleResponse: SimpleResponse
    {
        public List<Schedule> ScheduleList { get; set; } = new List<Schedule>();
    }
}
