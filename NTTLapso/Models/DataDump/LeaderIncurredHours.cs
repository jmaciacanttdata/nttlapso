namespace NTTLapso.Models.DataDump
{
    public class LeaderIncurredHours
    {
        public string id_employee { get; set; } = String.Empty;
        public string employee_name { get; set; } = String.Empty;
        public string service { get; set; } = String.Empty;
        public string task_id { get; set; } = String.Empty;
        public string date { get; set; } = String.Empty;
        public string incurred_hours { get; set; } = string.Empty;
    }

    public class LeaderIncurredHoursResponse : SimpleResponse
    {
        public List<LeaderIncurredHours> DataList { get; set; } = new List<LeaderIncurredHours>();
    }
}
