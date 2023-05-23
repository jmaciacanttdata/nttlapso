namespace NTTLapso.Models.Permissions
{
    public class EditPermissionRequest
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public bool? Registration { get; set; }
        public bool? Read { get; set; }
        public bool? Edit { get; set; }
        public bool? Delete { get; set; }
    }
}
