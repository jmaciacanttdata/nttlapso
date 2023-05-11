using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Process.UserCharge;
using NTTLapso.Models.Users;
using NTTLapso.Support_methods;

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

        public async Task<List<UserDataResponse>> List(UserListRequest? request)
        {
            List<UserDataResponse> response = new List<UserDataResponse>();
            string SQLQueryAux =
                "SELECT user.Id, `Name`, `Surnames`, `Email`, category.Id 'CategoryId', category.Value 'CategoryValue', user_schedule.Id 'ScheduleId', " +
                "user_schedule.Value 'ScheduleValue', team.Id 'TeamId', team.Team 'TeamValue', `UserName`, `UserPass`, `Active` ";
            string SQLQueryGeneral = 
                "FROM `user` " +
                "INNER JOIN user_schedule ON user.IdUserSchedule = user_schedule.Id " +
                "INNER JOIN category ON user.IdCategory = category.Id " +
                "INNER JOIN user_team ON user.Id = user_team.IdUser " +
                "INNER JOIN team ON user_team.IdTeam = team.Id " +
                "WHERE 1=1";
            if (request != null && request.Id > 0)
                SQLQueryGeneral += " AND user.Id={0}";
            if (request != null && request.Name != null && request.Name != "")
                SQLQueryGeneral += " AND Name LIKE '%{1}%'";
            if (request != null && request.Surnames != null && request.Surnames != "")
                SQLQueryGeneral += " AND Surnames LIKE '%{2}%'";
            if (request != null && request.Email != null && request.Email != "")
                SQLQueryGeneral += " AND Email LIKE '%{3}%'";
            if (request != null && request.IdCategory  > 0)
                SQLQueryGeneral += " AND category.Id = {4}";
            if (request != null && request.IdSchedule > 0)
                SQLQueryGeneral += " AND user_schedule.Id = {5}";

            if (request != null && request.IdTeam > 0) // If we filter by team
            {
                SQLQueryGeneral += " AND team.Id = {6}";
            }
            else
            {
                SQLQueryAux =
                "SELECT DISTINCT user.Id, `Name`, `Surnames`, `Email`, category.Id 'CategoryId', category.Value 'CategoryValue', user_schedule.Id 'ScheduleId', " +
                "user_schedule.Value 'ScheduleValue', `UserName`, `UserPass`, `Active` ";
            }

            if (request != null && request.UserName != null && request.UserName != "")
                SQLQueryGeneral += " AND UserName LIKE '%{7}%'";
            if (request != null && request.UserPass != null && request.UserPass != "")
                SQLQueryGeneral += " AND UserPass LIKE '%{8}%'";
            if (request != null && request.Active != null)
                SQLQueryGeneral += " AND Name = {9}";

            string SQLQuery = String.Format(SQLQueryAux + SQLQueryGeneral, request.Id, request.Name, request.Surnames, request.Email, request.IdCategory, request.IdSchedule, request.IdTeam, request.UserName, request.UserPass, request.Active);

            var userQuery = (await conn.QueryAsync(SQLQuery)).ToList();

            if (userQuery.Count != 0)
            {
                foreach (var user in userQuery)
                {
                    List<IdValue> teamList = new List<IdValue>();
                    UserDataResponse userDataResponse = new UserDataResponse();
                    userDataResponse.Id = user.Id;
                    userDataResponse.Name = user.Name;
                    userDataResponse.Surnames = user.Surnames;
                    userDataResponse.Email = user.Email;
                    userDataResponse.Category = new IdValue() { Id = user.CategoryId, Value = user.CategoryValue };
                    userDataResponse.Schedule = new IdValue() { Id = user.ScheduleId, Value = user.ScheduleValue };
                    if (request.IdTeam == null) // If we don't filter by team we make list of teams the user is in.
                    {
                        string SQLTeams = "SELECT team.Id, team.team 'Value' FROM user_team`user_team` " +
                                $"INNER JOIN team ON user_team.Idteam = team.Id WHERE IdUser = {user.Id} ";
                        List<IdValue> teams = (await conn.QueryAsync<IdValue>(SQLTeams)).ToList();
                        teamList = teams;
                    }
                    else
                    {
                        teamList.Add(new IdValue() { Id = user.TeamId, Value = user.TeamValue });
                    }
                    userDataResponse.TeamList = teamList;
                    userDataResponse.UserName = user.UserName;
                    userDataResponse.UserPass = user.UserPass;
                    userDataResponse.Active = user.Active == 1 ? true : false;

                    response.Add(userDataResponse);
                }
            }
            else
            {
                throw new Exception(message: "No results found");
            }

            return response;
        }

        public async Task<NewUserChargeRequest> Create(CreateUserRequest request)
        {
            NewUserChargeRequest response = new NewUserChargeRequest();
            string SQLQueryHeaders = "INSERT INTO user(`Name`, `Surnames`, `Email`, `IdCategory`, `IdUserSchedule`, `UserName`, `UserPass`, `Active`) ";
            string SQLQueryValues = "VALUES('{0}', '{1}', '{2}', {3}, {4}, '{5}', MD5('{6}'), {7})";

            string SQLQueryGeneral = String.Format((SQLQueryHeaders + SQLQueryValues), request.Name, request.Surnames, request.Email, request.IdCategory, request.IdUserSchedule, request.UserName, request.UserPass, request.Active);
            var rows = await conn.ExecuteAsync(SQLQueryGeneral);

            if (rows != 0) // If user is created
            {
                string SQLQuerGetId = "SELECT Id FROM user ORDER BY Id DESC LIMIT 1;";
                int idUser = (await conn.QueryAsync<int>(SQLQuerGetId)).FirstOrDefault();

                if (idUser != 0) // If Id correct
                {
                    string SQLGeneral = "INSERT INTO user_team(`IdUser`, `IdTeam`) VALUES({0}, {1});";
                    string SQLQuery = string.Format(SQLGeneral, idUser, request.IdTeam);
                    await conn.ExecuteAsync(SQLQuery);
                }

                response.IdUser = idUser;
                response.IdSchedule = request.IdUserSchedule;
                response.RegisterDate = DateTime.Now;
            }

            return response;
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
                SQLSet = check.CheckCommas(SQLSet);
            }

            SQLQueryPartial += SQLSet;
            SQLQueryPartial += " WHERE Id={0};";

            string SQLQueryGeneral = String.Format(SQLQueryPartial, request.Id, request.Name, request.Surnames, request.Email, request.IdCategory, request.IdUserSchedule, request.UserName, request.UserPass, request.Active);
            conn.Query(SQLQueryGeneral);
        }

        public async Task Delete(int Id)
        {
            string SQLQueryGeneral = String.Format("DELETE FROM user WHERE Id={0};", Id);
            conn.Query(SQLQueryGeneral);
        }

        public async Task ChangeUserState(ChangeUserStateRequest request)
        {
            string SQLQueryGeneral = String.Format("UPDATE user SET `Active`={1} WHERE Id={0}", request.Id, request.Active);
            conn.Query(SQLQueryGeneral);
        }
    }
}
