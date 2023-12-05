namespace NTTLapso.Models.DataDump
{
    public class IncurredHoursByDate
    {
        public string id_employee { get; set; }
        public string date { get; set; }
        public string incurred_hours { get; set; }
    }

    public class IncurredHoursByDateResponse : SimpleResponse
    {
        public List<IncurredHoursByDate> IncurredList { get; set; } = new List<IncurredHoursByDate>();
    }
}
