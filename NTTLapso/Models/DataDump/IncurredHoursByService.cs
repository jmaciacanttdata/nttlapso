namespace NTTLapso.Models.DataDump
{
    public class IncurredHoursByService
    {
        public string id_leader {  get; set; }
        public string id_employee { get; set;}
        public string employee_name { get; set; }
        public float remaining_hours {  get; set; }
    }

    public class IncurredHoursByServiceResponse
    {
        public bool Completed { get; set; }
        public int StatusCode { get; set; } = 200;
        public string Log { get; set; }
        public float TotalRemainingHours { get; set; } = 0;
        public List<IncurredHoursByService> DataList { get; set; } = new List<IncurredHoursByService>();
    }
}
