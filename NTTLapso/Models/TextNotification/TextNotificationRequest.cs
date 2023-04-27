namespace NTTLapso.Models.TextNotification
{
    public class TextNotificationRequest
    {
        public int IdNotification { get; set; }
        public string? Subject { get; set; }
        public string? Content { get; set; }
    }
}
