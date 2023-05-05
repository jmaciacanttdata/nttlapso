namespace NTTLapso.Models.Users
{
    public class CreateUserRequest
    {
        public string Name {  get; set; }
        public string? Surnames { get; set; }
        public string Email { get; set; }
        public int IdCategory { get; set; }
        public int IdUserSchedule { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public byte Active { get; set; }
    }
}
