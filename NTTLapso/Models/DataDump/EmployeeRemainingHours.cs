namespace NTTLapso.Models.DataDump
{
    public class EmployeeRemainingHours
    {
        public string id_employee {  get; set; }
        public string name { get; set; }
        public string service_team {  get; set; }

        public string incurred_hours_by_team {  get; set; }
        public string remaining_incurred_hours {  get; set; }
    }

    public class EmployeeRemainingHoursResponse
    {
        public bool Completed { get; set; }
        public int StatusCode { get; set; } = 200;
        public string Log {  get; set; }
        public List<EmployeeRemainingHours> EmployeesList { get; set; } = new List<EmployeeRemainingHours>();
    }
}
