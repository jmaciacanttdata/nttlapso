namespace NTTLapso.Models.DataDump
{
    public class SupervisedEmployeesResponse : SimpleResponse
    {
        public List<string> Employees { get; set; } = new List<string>();
    }
}
