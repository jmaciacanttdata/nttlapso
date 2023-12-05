namespace NTTLapso.Models.DataDump
{
    public class LeaderIncurredHours
    {
        public string id_supervisor {  get; set; }
        public string id_employee { get; set;}
        public string employee_name { get; set; }
        public float remaining_hours {  get; set; }
    }

    public class LeaderIncurredHoursResponse : SimpleResponse
    {
        public float TotalRemainingHours { get; set; } = 0;
        public List<LeaderIncurredHours> DataList { get; set; } = new List<LeaderIncurredHours>();
    }
}
