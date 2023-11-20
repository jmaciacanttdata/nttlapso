namespace NTTLapso.Models.DataDump
{
    public class DataDumpResponse
    {
        public bool? Completed { get; set; }
        public string? Message { get; set; }
        public List<Employee?> FailedEmployees {get;set;} = new List<Employee?>();

    }
}
