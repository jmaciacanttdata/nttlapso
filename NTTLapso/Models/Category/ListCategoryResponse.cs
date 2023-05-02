using NTTLapso.Models.General;

namespace NTTLapso.Models.Category
{
    public class ListCategoryResponse
    {
        public bool IsSuccess { get; set; }
        public List<IdValue> Data { get; set; }
        public Error Error { get; set; }
    }
}
