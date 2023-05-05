namespace NTTLapso.Models.Process.UserCharge
{
    public class UserChargeRequest
    {
        public int IdUser { get; set; }

        public int Year { get; set; }

        public int TotalVacationDays { get; set; }

        public int TotalCompensatedDays { get; set; }
    }
}
