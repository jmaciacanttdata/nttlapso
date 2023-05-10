namespace NTTLapso.Models.Process.EmailConfig
{
    public class SmtpEmailConfig
    {
        public string Sender { get; set; }

        public string User { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public string Server { get; set; }

        public int Port { get; set; }

        public bool IsSSL { get; set; }


    }
}
