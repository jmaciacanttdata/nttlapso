namespace NTTLapso.Models.Vacations
{
    public class EditLogRequest
    {
        public int? IdVacation { get; set; }
        public int? IdUserState { get; set; }
        public int? IdState { get; set; }
        public DateTime? OldPetitionDate { get; set; }
        public DateTime? StateDate { get; set; }
        public string? Detail { get; set; }
    }
}
