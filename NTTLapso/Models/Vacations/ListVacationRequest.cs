namespace NTTLapso.Models.Vacations
{
    public class ListVacationRequest
    {
        public int? IdUserPetition { get; set; }
        public DateTime PetitionDate { get; set; }
        public int? IdPetitionType { get; set; }
    }
}
