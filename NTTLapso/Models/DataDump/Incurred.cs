namespace NTTLapso.Models.DataDump
{
    public class Incurred
    {
        public string id_employee { get; set; }
        public string nombre_persona { get; set; }
        public string situacion_actual_persona { get; set; }
        public string pkey_jira { get; set; }
        public string component { get; set; }
        public string group { get; set; }
        public string service_line { get; set; }
        public string task_type { get; set; }
        public bool facturable { get; set; }
        public string task_id { get; set; }
        public string task_summary { get; set; }
        public string task_status { get; set; }
        public string task_origin { get; set; }
        public float internal_estimation { get; set; }
        public int agile_estimation { get; set; }
        public string estimation_unit { get; set; }
        public string sub_task_type { get; set; }
        public string typology { get; set; }
        public string sub_task_id { get; set; }
        public string sub_task_summary { get; set; }
        public string sub_task_status { get; set; }
        public string sub_task_origin { get; set; }
        public string incurred_comment { get; set; }
        public double sub_task_estimation { get; set; }
        public float incurred_hours { get; set; }
        public double etc { get; set; }
        public DateTime fecha { get; set; }
        public double month_date { get; set; }
    }
}
