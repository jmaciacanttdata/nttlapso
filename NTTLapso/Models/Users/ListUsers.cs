using NTTLapso.Models.General;
using NTTLapso.Models.Users;

namespace NTTLapso.Models.Users
{
    public class ListUsers
    {
        public bool IsSuccess { get; set; }
        public List<UserDataResponse> Data { get; set; }
        public Error Error { get; set; }
    }
}
