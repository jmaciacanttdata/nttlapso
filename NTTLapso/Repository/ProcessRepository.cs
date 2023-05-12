using Dapper;
using MySqlConnector;
using NTTLapso.Models.Process.UserCharge;
using System.Net.Mail;
using System.Net;
using NTTLapso.Models.Process.EmailConfig;
using NTTLapso.Service;
using NTTLapso.Models.Mail;

namespace NTTLapso.Repository
{
    public class ProcessRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private IConfiguration _config;
        private MySqlConnection conn;
        private UserService _userService;

        public ProcessRepository(IConfiguration conf)
        {
            _config = conf;
            conn = new MySqlConnection(connectionString);
            _userService = new UserService();
        }

        // Method for inserting user's vacation and compensated days at beginning of year.
        public async Task SetUsersCharge(UserChargeRequest request)
        {
            string SQLQueryGeneral = String.Format("INSERT INTO user_charge(`IdUser`, `Year`, `TotalVacationDays`, `TotalCompensatedDays`) VALUES({0}, {1}, {2}, {3})", request.IdUser, request.Year, request.TotalVacationDays, request.TotalCompensatedDays);
            await conn.QueryAsync(SQLQueryGeneral);
        }

        // Method for inserting new user's vacation and compensated days for the remainder of the year.
        public async Task SetNewUserCharge(UserChargeRequest request)
        {
            string SQLQuery = String.Format("SELECT `IdUser` FROM user_charge WHERE `IdUser` = {0} AND `Year` = {1};", request.IdUser, request.Year);

            var response = await conn.QueryAsync(SQLQuery); // Check if user charge exists

            if (response.Count() == 0) // If doesn't exist we insert user charge.
            {
                string SQLQueryGeneral = String.Format("INSERT INTO user_charge(`IdUser`, `Year`, `TotalVacationDays`, `TotalCompensatedDays`) VALUES({0}, {1}, {2}, {3})", request.IdUser, request.Year, request.TotalVacationDays, request.TotalCompensatedDays);
                await conn.QueryAsync(SQLQueryGeneral);
            }
            else
            {
                throw new Exception(message: $"The user with id: {request.IdUser} already has a charge asigned for the year: {request.Year}");
            }
        }

        internal async Task SendNotification(MailSender sender)
        {
            foreach(MailReplacer text in sender.Replacers) // Replace names, content... in email.
            {
                sender.Content.Content = sender.Content.Content.Replace(text.SearchText, text.ReplaceText);
            }

            if(SendEmail(sender)) // If email send, we add register to database.
            {
                string SQLTextGeneral = "INSERT INTO send_notification(`IdTextNotification`, `IdUser`, `SendDate`) VALUES({0}, {1}, NOW())";
                string SQLText = string.Format(SQLTextGeneral, sender.Content.IdNotificationType, sender.Receiver.Id);
                await conn.ExecuteAsync(SQLText);
            }
        }

        internal bool SendEmail(MailSender sender)
        {
            try
            {
                SmtpEmailConfig emailConfig = _config.GetSection("SmtpEmailConfig").Get<SmtpEmailConfig>();

                MailAddress from = new MailAddress(emailConfig.Sender, emailConfig.Name); // Create sender address
                MailAddress to = new MailAddress(sender.Receiver.Email, sender.Receiver.Name); // Create receiver address

                MailMessage mailMessage = new MailMessage(from, to);
                foreach (var CC in sender.ReceiverCCList) // Add all Copy adresses.
                {
                    mailMessage.CC.Add(new MailAddress(CC.Email, CC.Name));
                }
                mailMessage.Subject = sender.Content.Subject;
                mailMessage.Body = sender.Content.Content;
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient(emailConfig.Server, emailConfig.Port);
                smtpClient.Credentials = new NetworkCredential(emailConfig.User, emailConfig.Password);
                smtpClient.EnableSsl = emailConfig.IsSSL;
                smtpClient.Send(mailMessage);

                return true;

            }catch(Exception ex)
            {
                throw new Exception(message: "Couldn't send email notification");
                return false;
            }
        }
    }
}
