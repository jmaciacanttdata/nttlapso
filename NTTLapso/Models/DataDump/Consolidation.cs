namespace NTTLapso.Models.DataDump
{
    public class Consolidation
    {
        public string id_employee {  get; set; }
        public string name { get; set; }
        public string service { get; set; }
        public string service_team { get; set; }
        public bool NotEmployees {  get; set; }
        public bool NotSchedules { get; set; }
        public bool NotIncurred {  get; set; }
    }
}
