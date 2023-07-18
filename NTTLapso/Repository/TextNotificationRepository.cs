using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;
using NTTLapso.Models.TextNotification;

namespace NTTLapso.Repository
{
    public class TextNotificationRepository
    {
        private static string connectionString;
        private MySqlConnection conn;
        private IConfiguration _config;
        public TextNotificationRepository(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetValue<string>("ConnectionStrings:Develop");
            conn = new MySqlConnection(connectionString);
            _config = config;
        }

        public async Task<List<TextNotificationData>> List(IdTextNotificationRequest? request)
        {
            List<TextNotificationData> response = new List<TextNotificationData>();

            string SQLQueryGeneral = "SELECT Id as IdNotification, Subject, Content FROM text_notification WHERE 1=1";
            if (request != null && request.Id > 0)
                SQLQueryGeneral += " AND Id={0}";
            if (request != null && request.Subject != null && request.Subject != "")
                SQLQueryGeneral += " AND Subject LIKE '%{1}%'";
            if (request != null && request.Content != null && request.Content != "")
                SQLQueryGeneral += " AND Content LIKE '%{2}%'";

            string SQLQuery = String.Format(SQLQueryGeneral, request.Id, request.Subject, request.Content);

            response = conn.Query<TextNotificationData>(SQLQuery).ToList();
            return response;
        }

        public async Task Create(TextNotificationRequest request)
        {
            string SQLQueryGeneral = String.Format("INSERT INTO text_notification(Subject, Content) VALUES('{0}', '{1}')", request.Subject, request.Content);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Edit(IdTextNotificationRequest request)
        {
            string SQLQueryGeneral = String.Format("UPDATE text_notification SET Subject='{1}', Content='{2}' WHERE Id={0}", request.Id, request.Subject, request.Content);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Delete(int Id)
        {
            string SQLQueryGeneral = String.Format("DELETE FROM text_notification WHERE Id={0};", Id);
            conn.Query(SQLQueryGeneral);

        }
    }
}
