namespace NTTLapso.Models.DataDump
{
    public class MonthlyIncurredHours
    {
        public string IdEmployee { get; set; } = string.Empty;
        public string Year { get; set; }
        public string Month { get; set; }
        public string TotalHours { get; set; }
        public string TotalIncurredHours { get; set; }
    }
}
