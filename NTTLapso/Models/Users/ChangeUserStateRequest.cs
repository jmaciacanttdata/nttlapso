using NTTLapso.Models.General;

namespace NTTLapso.Models.Users
{
    public class ChangeUserStateRequest
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public int IdApprover { get; set; }
        public string NameApprover { get; set; }
    }
}
