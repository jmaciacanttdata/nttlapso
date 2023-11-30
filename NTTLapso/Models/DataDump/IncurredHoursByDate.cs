namespace NTTLapso.Models.DataDump
{
    public class IncurredHoursByDate
    {
        public string id_employee { get; set; }
        public string date { get; set; }
        public string incurred_hours { get; set; }
    }

    public class IncurredHoursByDateResponse
    {
        public bool Completed { get; set; }
        public int StatusCode { get; set; } = 200;
        public string Log { get; set; }
        public List<IncurredHoursByDate> IncurredList { get; set; } = new List<IncurredHoursByDate>();
    }
}
