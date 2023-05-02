using NTTLapso.Models.General;

namespace NTTLapso.Models.PetitionStatus
{
    public class ListPetitionStatusResponse
    {
        public bool IsSuccess { get; set; }
        public List<IdValue> Data { get; set; }
        public Error Error { get; set; }
    }
}
