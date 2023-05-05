using NTTLapso.Models.Permissions;
using Dapper;
using MySqlConnector;
using NTTLapso.Models.Permissions;
using NTTLapso.Support_methods;
using NTTLapso.Models.Users;

namespace NTTLapso.Repository
{
    public class UserRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;
        private SupportMethods check = new SupportMethods();
        public UserRepository()
        {
            conn = new MySqlConnection(connectionString);
        }

        public async Task<List<UserDataResponse>> List(UserDataResponse? request)
        {
            List<UserDataResponse> response = new List<UserDataResponse>();

            string SQLQueryGeneral = "SELECT user.Id, `Name`, `Surnames`, `Email`, category.Value 'Category', user_schedule.Value 'Schedule', `UserName`, `UserPass`, `Active` FROM user INNER JOIN user_schedule ON user.IdUserSchedule = user_schedule.Id INNER JOIN category ON user.IdCategory = category.Id WHERE 1=1";
            if (request != null && request.Id > 0)
                SQLQueryGeneral += " AND user.Id={0}";
            if (request != null && request.Name != null && request.Name != "")
                SQLQueryGeneral += " AND Name LIKE '%{1}%'";
            if (request != null && request.Surnames != null && request.Surnames != "")
                SQLQueryGeneral += " AND Surnames LIKE '%{2}%'";
            if (request != null && request.Email != null && request.Email != "")
                SQLQueryGeneral += " AND Email LIKE '%{3}%'";
            if (request != null && request.Category != null )
                SQLQueryGeneral += " AND category.Value LIKE '%{4}%'";
            if (request != null && request.Schedule != null)
                SQLQueryGeneral += " AND user_schedule.Value LIKE '%{5}%'";
            if (request != null && request.UserName != null && request.UserName != "")
                SQLQueryGeneral += " AND UserName LIKE '%{6}%'";
            if (request != null && request.UserPass != null && request.UserPass != "")
                SQLQueryGeneral += " AND UserPass LIKE '%{7}%'";
            if (request != null && request.Active != null)
                SQLQueryGeneral += " AND Name = {8}";

            string SQLQuery = String.Format(SQLQueryGeneral, request.Id, request.Name, request.Surnames, request.Email, request.Category, request.Schedule, request.UserName, request.UserPass, request.Active);

            response = conn.Query<UserDataResponse>(SQLQuery).ToList();

            if (response.Count == 0)
            {
                throw new Exception(message: "No results found");
            }
            return response;
        }
        public async Task Create(CreateUserRequest request)
        {
            string SQLQueryHeaders = "INSERT INTO user(`Name`, `Surnames`, `Email`, `IdCategory`, `IdUserSchedule`, `UserName`, `UserPass`, `Active`) ";
            string SQLQueryValues = "VALUES('{0}', '{1}', '{2}', {3}, {4}, '{5}', MD5('{6}'), {7})";

            string SQLQueryGeneral = String.Format((SQLQueryHeaders + SQLQueryValues), request.Name, request.Surnames, request.Email, request.IdCategory, request.IdUserSchedule, request.UserName, request.UserPass, request.Active);
            conn.Query(SQLQueryGeneral);

        }

        public async Task Edit(EditUserRequest request)
        {
            string SQLSet = "";
            string SQLQueryPartial = "UPDATE user SET";
            if (request.Name != null && request.Name != "")
                SQLSet += " Name='{1}'";

            if (request.Surnames != null)
                SQLSet += ", `Surnames`= '{2}'";

            if (request.Email != null)
                SQLSet += ", `Email`= '{3}'";

            if (request.IdCategory != null)
                SQLSet += ", `IdCategory`={4}";

            if (request.IdUserSchedule != null)
                SQLSet += ", `IdUserSchedule`={5}";

            if (request.UserName != null)
                SQLSet += ", `UserName`= '{6}'";

            if (request.UserPass != null)
                SQLSet += ", `UserPass`=  MD5('{7}')";

            if (request.Active != null)
                SQLSet += ", `Active`={8}";

            if (SQLSet != "")
            {
                /*char[] stringArray = SQLSet.ToCharArray();
                if (stringArray[0] == ',')
                    SQLSet = SQLSet.Substring(1, SQLSet.Length - 1);*/
                SQLSet = check.CheckCommas(SQLSet);
            }

            SQLQueryPartial += SQLSet;
            SQLQueryPartial += " WHERE Id={0};";

            string SQLQueryGeneral = String.Format(SQLQueryPartial, request.Id, request.Name, request.Surnames, request.Email, request.IdCategory, request.IdUserSchedule, request.UserName, request.UserPass, request.Active);
            conn.Query(SQLQueryGeneral);

        }

        public async Task ChangeUserState(ChangeUserStateRequest request)
        {

            string SQLQueryGeneral = String.Format("UPDATE user SET `Active`={1} WHERE Id={0}", request.Id, request.Active);
            conn.Query(SQLQueryGeneral);

        }
        public async Task Delete(int Id)
        {
            string SQLQueryGeneral = String.Format("DELETE FROM user WHERE Id={0};", Id);
            conn.Query(SQLQueryGeneral);
        }
    }
}
