using NTTLapso.Models.General;

namespace NTTLapso.Models.TextNotification
{
    public class ListTextNotificationResponse
    {
        public bool IsSuccess { get; set; }
        public List<TextNotificationData> Data { get; set; }
        public Error? Error { get; set; }
    }
}
