using NTTLapso.Models.TextNotification;

namespace NTTLapso.Models.PetitionStatus
{
    public class PetitionStatusDataResponse
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public TextNotificationData TextNotification { get; set; } = new TextNotificationData();
    }
}
