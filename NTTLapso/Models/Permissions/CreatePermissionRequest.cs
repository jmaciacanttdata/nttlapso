namespace NTTLapso.Models.Permissions
{
    public class CreatePermissionRequest
    {
        public string Value { get; set; }
        public bool Registration { get; set; }
        public bool Read { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
    }
}
