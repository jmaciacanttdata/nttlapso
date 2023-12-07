namespace NTTLapso.Models.Users
{
    public class CreateUserRequest
    {
        public string Name {  get; set; }
        public string Email { get; set; }
        public int IdCategory { get; set; }
        public int IdUserSchedule { get; set; }
        public int IdTeam { get; set; }
        public int IdRol { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public bool Active { get; set; }
    }
}
