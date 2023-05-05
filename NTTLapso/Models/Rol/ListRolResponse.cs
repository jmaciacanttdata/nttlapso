using NTTLapso.Models.General;

namespace NTTLapso.Models.Rol
{
    public class ListRolResponse
    {
        public bool IsSuccess { get; set; }
        public List<IdValue> Data { get; set; }
        public Error Error { get; set; }
    }
}
