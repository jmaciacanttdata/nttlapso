using NTTLapso.Models.General;

namespace NTTLapso.Models.PetitionType
{
    public class ListPetitionTypeResponse
    {
        public bool IsSuccess { get; set; }
        public List<PetitionTypeDataResponse> Data { get; set; }
        public Error Error { get; set; }
    }
}
