using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NTTLapso.Models.DataDump
{
    public class Incurred
    {
        public string id_employee { get; set; } = string.Empty;
        public string service_name { get; set; } = string.Empty;
        public string service_team { get; set; } = string.Empty;
        public string pkey_jira { get; set; } = string.Empty;
        public string component { get; set; } = string.Empty;
        public string grouping { get; set; } = string.Empty;
        public string service_line { get; set; } = string.Empty;
        public string task_type { get; set; } = string.Empty;
        public string billable_to_customer { get; set; } = string.Empty;
        public string task_id { get; set; } = string.Empty;
        public string task_summary { get; set; } = string.Empty;
        public string task_state { get; set; } = string.Empty;
        public string task_origin { get; set; } = string.Empty;
        public string intern_estimation { get; set; } = string.Empty;
        public string agile_estimation { get; set; } = string.Empty;
        public string estimation_unit { get; set; } = string.Empty;
        public string subtask_type { get; set; } = string.Empty;
        public string typology { get; set; } = string.Empty;
        public string subtask_id { get; set; } = string.Empty;
        public string subtask_summary { get; set; } = string.Empty;
        public string subtask_state { get; set; } = string.Empty;
        public string subtask_origin { get; set; } = string.Empty;
        public string incurred_comment { get; set; } = string.Empty;
        public string subtask_estimation { get; set; } = string.Empty;
        public string incurred_hours { get; set; } = string.Empty;
        public string date { get; set; } = string.Empty;
    }
}
