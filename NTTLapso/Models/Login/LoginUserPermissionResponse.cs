namespace NTTLapso.Models.Login
{
    public class LoginUserPermissionResponse
    {
        public int TeamId { get; set; }
        public string Value { get; set; }
        public bool Registration { get; set; }
        public bool Read { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
    }
}
