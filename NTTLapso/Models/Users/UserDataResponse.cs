using NTTLapso.Models.General;

namespace NTTLapso.Models.Users
{
    public class UserDataResponse
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Surnames { get; set; }
        public string? Email { get; set; }
        public IdValue? Category { get; set; }
        public IdValue? Schedule { get; set; }
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public bool? Active { get; set; }
    }
}
