namespace NTTLapso.Models.Users
{
    public class ChangeUserStateRequest
    {
        public int Id { get; set; }
        public byte Active { get; set; }
    }
}
