namespace NTTLapso.Models.Vacations
{
    public class CreateVacationRequest
    {
        public int IdUserPetition { get; set; }
        public DateTime Day { get; set; }
        public int IdPetitionType { get; set; }
    }
}
