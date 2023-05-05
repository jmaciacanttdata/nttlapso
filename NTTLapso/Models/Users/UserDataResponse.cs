namespace NTTLapso.Models.Users
{
    public class UserDataResponse
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Surnames { get; set; }
        public string? Email { get; set; }
        public string? Category { get; set; }
        public string? Schedule { get; set; }
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public byte? Active { get; set; }
    }
}
