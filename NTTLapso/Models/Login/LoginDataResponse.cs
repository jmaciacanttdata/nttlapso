namespace NTTLapso.Models.Login
{
    public class LoginDataResponse
    {
        public LoginResponse Data { get; set; }
        public DateTime DateLogin { get; set; }
        public DateTime DateLoginExpires { get; set; }
        public string Token { get; set; }
    }
}
