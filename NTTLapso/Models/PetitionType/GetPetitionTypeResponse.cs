using NTTLapso.Models.General;

namespace NTTLapso.Models.PetitionType
{
    public class GetPetitionTypeResponse
    {
        public PetitionTypeDataResponse Data { get; set; } = new PetitionTypeDataResponse();

        public ErrorResponse Error { get; set; } = new ErrorResponse();
    }
}
