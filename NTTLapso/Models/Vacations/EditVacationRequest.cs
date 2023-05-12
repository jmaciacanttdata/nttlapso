namespace NTTLapso.Models.Vacations
{
    public class EditVacationRequest
    {
        public int IdUserPetition { set; get; }
        public DateTime OldPetitionDate { set; get; }
        public DateTime PetitionDate { set; get; }
        public int? IdPetitionType { set; get; }
    }
}
