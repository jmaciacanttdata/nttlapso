namespace NTTLapso.Models.Users
{
    public class EditUserRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public int? IdCategory { get; set; }
        public int? IdUserSchedule { get; set; }
        public string? UserName { get; set; }
        public string? UserPass { get; set; }
        public bool? Active { get; set; }
    }
}
