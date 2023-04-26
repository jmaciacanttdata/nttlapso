using NTTLapso.Models.General;

namespace NTTLapso.Models.Categories
{
    public class CategoriesResponse
    {
        public bool isSuccess { get; set; }
        //public ErrorResponse? Error { get; set; }
        public string? ErrorType { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
