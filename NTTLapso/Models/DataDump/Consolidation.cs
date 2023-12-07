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
    public class ConsolidationResponse : SimpleResponse
    {
        public List<Consolidation> ConsolidatedEmployees { get; set; } = new List<Consolidation>();
    }

    public class NumConsolidationResponse : SimpleResponse
    {
        public int NumConsolidate { get; set; }
    }
}
