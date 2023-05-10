using NTTLapso.Models.General;
using NTTLapso.Models.Users;

namespace NTTLapso.Models.Mail
{
    public class MailSender
    {
        public UserMail Receiver { get; set; } = new UserMail();

        public List<UserMail>? ReceiverCCList { get; set; } = new List<UserMail>();

        public MailContent Content { get; set; } = new MailContent();

        public List<MailReplacer> Replacers { get; set; }
    }
}
