namespace NTTLapso.Models.PetitionStatus
{
    public class EditPetitionStatusRequest
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int? IdTextNotification { get; set; }
    }
}
