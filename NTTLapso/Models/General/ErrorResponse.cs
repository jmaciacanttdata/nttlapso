namespace NTTLapso.Models.General
{
    public class ErrorResponse
    {
        public bool IsSuccess { get; set; }

        public string ErrorType { get; set; }

        public string ErrorMessage { get; set; }
    }
}
