using NTTLapso.Models.General;

namespace NTTLapso.Models.Categories
{
    public class CategoriesDataResponse
    { 
        public CategoriesResponse Data { get; set; }
        public byte isSuccessful { get; set; }
        public ErrorResponse? Error { get; set; }
    }
}
