namespace NTTLapso.Models.Process.UserCharge
{
    public class NewUserChargeRequest
    {
        public int IdUser { get; set; }

        public int IdSchedule { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
