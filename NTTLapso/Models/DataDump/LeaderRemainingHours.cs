namespace NTTLapso.Models.DataDump
{
    public class LeaderRemainingHours
    {
        public string id_supervisor {  get; set; }
        public string id_employee { get; set;}
        public string employee_name { get; set; }
        public float remaining_hours {  get; set; }
    }

    public class LeaderRemainingHoursResponse : SimpleResponse
    {
        public float TotalRemainingHours { get; set; } = 0;
        public List<LeaderRemainingHours> DataList { get; set; } = new List<LeaderRemainingHours>();
    }
}
