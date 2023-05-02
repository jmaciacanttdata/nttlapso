using NTTLapso.Models.General;

namespace NTTLapso.Models.TextNotification
{
    public class TextNotificationResponse
    {
        public bool IsSuccess { get; set; }
        public Error? Error { get; set; }
    }
}
