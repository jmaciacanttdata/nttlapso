namespace NTTLapso.Models.Vacations
{
    public class VacationStateLogListRequest
    {
        public int? IdVacation { get; set; }
        public int IdUser { get; set; }

        public int IdPetitionType { get; set; }

        public int IdPetitionState { get; set; }

        public DateTime PetitionDate { get; set; }

        public DateTime StateDate { get; set; }
    }
}
