namespace NTTLapso.Models.Permissions
{
    public class PermissionRequest
    {
        public int Id { get; set; }
        public string? Value { get; set; }
        public byte? Registration { get; set; }
        public byte? Read { get; set; }
        public byte? Edit { get; set; }   
        public byte? Delete { get; set; }

    }
}
