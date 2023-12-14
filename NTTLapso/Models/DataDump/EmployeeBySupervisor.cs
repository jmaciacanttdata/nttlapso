namespace NTTLapso.Models.DataDump
{
    public class EmployeeBySupervisor
    {
        public int Id { get; set; }
    }

    public class EmployeeBySupervisorResponse : SimpleResponse
    {
        public List<EmployeeBySupervisor> EmployeesList { get; set; }
    }
}
