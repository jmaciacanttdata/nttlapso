using NTTLapso.Models.General;

namespace NTTLapso.Models.PetitionType
{
    public class GetPetitionTypeListResponse
    {
        public List<PetitionTypeDataResponse> Data { get; set; } = new List<PetitionTypeDataResponse>();

        public ErrorResponse Error { get; set; } = new ErrorResponse();
    }
}
