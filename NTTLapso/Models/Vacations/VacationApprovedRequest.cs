namespace NTTLapso.Models.Vacations
{
    public class VacationApprovedRequest
    {
        public int IdUserState { get; set; }
        public DateTime StateDate { get; set; }
        public int IdPetitionState { get; set; }
        public string? Detail { get; set; }
    }
}
