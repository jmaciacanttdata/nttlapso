namespace NTTLapso.Models.DataDump
{
    public class MonthlyIncurredHours
    {
        public string IdEmployee { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public float TotalHours { get; set; }
        public float TotalIncurredHours { get; set; }
        public float HoursDiff { get; set; }
    }
}
