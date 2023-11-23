namespace NTTLapso.Models.DataDump
{
    public class ConsolidationResponse
    {
        public bool Completed {  get; set; }
        public string Log { get; set; }
        public List<Consolidation> ConsolidatedEmployees { get; set; } = new List<Consolidation>();
    }
}
