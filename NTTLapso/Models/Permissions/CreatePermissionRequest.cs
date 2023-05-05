namespace NTTLapso.Models.Permissions
{
    public class CreatePermissionRequest
    {
        public string Value { get; set; }
        public byte Registration { get; set; }
        public byte Read { get; set; }
        public byte Edit { get; set; }
        public byte Delete { get; set; }
    }
}
