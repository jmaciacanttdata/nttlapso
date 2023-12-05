namespace NTTLapso.Models.DataDump
{
    public class EmployeeRemainingHours
    {
        public string id_employee {  get; set; }
        public string name { get; set; }
        public string surnames { get; set; }
        public string service_team {  get; set; }

        public string incurred_hours_by_team {  get; set; }
        public string remaining_incurred_hours {  get; set; }
    }

    public class EmployeeRemainingHoursResponse : SimpleResponse
    {
        public List<EmployeeRemainingHours> EmployeesList { get; set; } = new List<EmployeeRemainingHours>();
    }
}
