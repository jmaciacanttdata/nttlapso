using System;
using System.Net;

namespace NTTLapso.Models.General
{
    public class Error
    {
        public string Message { get; set; }
        public Exception InnerMessage { get; set; }

        public Error(Exception ex)
        {
            this.Message = ex.Message;
            this.InnerMessage = ex.InnerException;
        }

        public Error(string Message, Exception? ex)
        {
            this.Message = Message;
            if(ex != null)
                this.InnerMessage = ex.InnerException;
        }
    }
}
