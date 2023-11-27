namespace NTTLapso.Models.DataDump
{
    public class EmployeeMonthlyIncurredHours
    {
        public string id_employee {  get; set; }
        public string name { get; set; }
        public string total_incurred_hours { get; set; }
    }

    public class EmployeeMonthlyIncurredHoursResponse 
    {
        public bool Completed { get; set; }
        public string Log {  get; set; }
        public List<EmployeeMonthlyIncurredHours> EmployeesList { get; set; } = new List<EmployeeMonthlyIncurredHours>();
    }
}
