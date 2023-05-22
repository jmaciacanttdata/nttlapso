using NTTLapso.Models.General;

namespace NTTLapso.Models.Vacations
{
    public class PercentagePerMonthResponse
    {
        public bool IsSuccess { set; get; }
        public List<PercentagePetitionDay> Data {set; get;}
        public Error Error { set; get;}
    }
}
