namespace NTTLapso.Models.DataDump
{
    public class MonthlyIncurredHours
    {
        public string NumEmpleado { get; set; } = string.Empty;
        public int Anyo { get; set; }
        public int Mes { get; set; }
        public float HorasIncurrir { get; set; }
        public float HorasIncurridas { get; set; }
    }
}
