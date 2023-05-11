namespace NTTLapso.Models.Vacations
{
    public class CreateLogRequest
    {
        public int IdVacation { get; set; }
        public int IdUserState { get; set; }
        public int IdState { get; set; }
        public string? Detail { get; set; }
    }
}
