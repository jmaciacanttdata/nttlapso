namespace NTTLapso.Models.TextNotification
{
    public class TextNotificationResponse
    {
        public bool isSuccess { get; set; }
        //public ErrorResponse? Error { get; set; }
        public string? ErrorType { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
