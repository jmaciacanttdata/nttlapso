using NTTLapso.Models.General;

namespace NTTLapso.Models.Users
{
    public class UserListRequest
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Surnames { get; set; }
        public string? Email { get; set; }
        public int? IdCategory { get; set; }
        public int? IdSchedule { get; set; }
        public int? IdTeam { get; set; }
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public bool? Active { get; set; }
    }
}
