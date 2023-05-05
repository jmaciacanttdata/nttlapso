using NTTLapso.Models.General;

namespace NTTLapso.Models.Team
{
    public class ListTeamResponse
    {
        public bool IsSuccess { get; set; }
        public List<TeamData> Data { get; set; }
        public Error Error { get; set; }
    }
}
