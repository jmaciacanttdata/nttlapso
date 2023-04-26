using Dapper;
using MySqlConnector;
using NTTLapso.Models.Login;
using NTTLapso.Models.TextNotification;

namespace NTTLapso.Repository
{
    public class MastersRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public MastersRepository() { 
            conn = new MySqlConnection(connectionString);
        }

        public async Task<TextNotificationResponse> SetTextNotification(TextNotificationRequest textNotificationData)
        {
            TextNotificationResponse resp = new TextNotificationResponse();
            string SQLQuery = "INSERT INTO text_notification (`Subject`, `Content`) VALUES ('"+ textNotificationData.Subject +"', '"+textNotificationData.Content+"')";
            resp = conn.Query(SQLQuery).FirstOrDefault();
            return resp;
        }
        public async Task<TextNotificationDataResponse?> GetTextNotification(TextNotificationRequest textNotificationRequest)
        {
            TextNotificationDataResponse resp = new TextNotificationDataResponse();
            string SQLQuery = "SELECT Id, `Subject`, `Content` FROM text_notification WHERE Id = '"+textNotificationRequest.IdNotification+"'";
            resp = conn.Query(SQLQuery).FirstOrDefault();
            return resp;
        }
        public async Task<TextNotificationResponse> UpdateTextNotification(TextNotificationRequest textNotificationData)
        {
            TextNotificationResponse resp = new TextNotificationResponse();
            string SQLQuery = "UPDATE text_notification SET Subject = '"+textNotificationData.Subject+"', Content = '"+textNotificationData.Content+"' WHERE Id = '"+textNotificationData.IdNotification+"'";
            resp = conn.Query(SQLQuery).FirstOrDefault();
            return resp;
        }
        public async Task<TextNotificationResponse> DeleteTextNotification(TextNotificationRequest textNotificationData)
        {
            TextNotificationResponse resp = new TextNotificationResponse();
            string SQLQuery = "DELETE FROM text_notification WHERE Id = '"+textNotificationData.IdNotification+"'";
            resp = conn.Query(SQLQuery).FirstOrDefault();
            return resp;
        }
    }
}
