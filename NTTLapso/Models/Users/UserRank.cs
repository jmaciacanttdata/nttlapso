using NTTLapso.Models.DataDump;
using NTTLapso.Models.General;

namespace NTTLapso.Models.Users
{
    public class UserRank
    {
        public int? id_user { get; set; }
        public string? user_name { get; set; }
        public string? service {  get; set; }
        public string? service_team { get; set; }
        public int? user_rank {  get; set; }
    }

    public class UserRankResponse : SimpleResponse
    {
        public List<UserRank> DataList { get; set; } = new List<UserRank>();
    }
}
