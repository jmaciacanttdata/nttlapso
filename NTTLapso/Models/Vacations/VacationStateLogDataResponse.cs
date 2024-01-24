using NTTLapso.Models.General;

namespace NTTLapso.Models.Vacations
{
    public class VacationStateLogDataResponse
    {
        public int IdVacation { get; set; }
        public IdValue User { get; set; } = new IdValue();

        public IdValue PetitionType { get; set; } = new IdValue();

        public IdValue PetitionState { get; set; } = new IdValue();

        public DateTime PetitionDate { get; set; }

        public DateTime StateDate { get; set; }
        public string Service {  get; set; }

        public string Detail { get; set; }
    }
}
