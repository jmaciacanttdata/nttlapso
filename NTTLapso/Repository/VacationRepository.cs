using Dapper;
using MySqlConnector;
using NTTLapso.Models.General;
using NTTLapso.Models.Vacations;
using System;
using System.Reflection;

namespace NTTLapso.Repository
{
    public class VacationRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public VacationRepository()
        {
            conn = new MySqlConnection(connectionString);
        }

        public async Task Create(CreateVacationRequest request)
        {

            string SQLQuery = "INSERT INTO vacation (`IdUserPetition`, `PetitionDate`, `IdPetitionType`) VALUES ({0}, '{1}', {2})";
            string SQLQueryGeneral = String.Format(SQLQuery, request.IdUserPetition, request.Day.ToString("yyyy-MM-dd"), request.IdPetitionType);

            conn.Query(SQLQueryGeneral);
            CreateLogRequest requestLog = new CreateLogRequest();
            requestLog.IdVacation = conn.ExecuteScalar<int>("SELECT id FROM vacation WHERE IdUserPetition = "+ request.IdUserPetition);
            requestLog.IdUserState = request.IdUserPetition;
            requestLog.IdState = 1;
            requestLog.StateDate = request.Day;
            requestLog.Detail = "";
            CreateLog(requestLog);
        }

        public async Task<bool> CheckViability(CreateVacationRequest request, int idTeam)
        {
            int result;
            DateTime date = new DateTime();
            date = request.Day.Date;
            bool teamViability = true;
            string fecha = date.ToString("yyyy-MM-dd");
            string checkIfIsSelected = "SELECT IdUser FROM user_team INNER JOIN vacation ON IdUser =IdUserPetition WHERE petitionDate = '" + fecha + "'";
            List<int> idUsers = conn.Query<int>(checkIfIsSelected).ToList();
            foreach (int idUser in idUsers)
            {
                //if user have aproved this date then it will be not available 
                if (idUser == request.IdUserPetition)
                {
                    teamViability = false;
                }
            }
            var resulta = conn.Query(string.Format("CALL SP_GET_VACATIONS_VIABILITY({0},'{1}')", request.IdUserPetition, fecha));
            if (resulta.Count() > 0)
            {
                teamViability = false;
            }
            return teamViability;
        }

        public async Task CreateLog(CreateLogRequest request)
        {
            string SQLQuery = "INSERT INTO vacation_state_log (`IdVacation`, `IdUserState`, `IdState`, `StateDate`, `Detail`) VALUES ({0}, {1}, {2}, '{3}', '{4}')";
            string SQLQueryGeneral = String.Format(SQLQuery, request.IdVacation, request.IdUserState, request.IdState,request.StateDate.ToString("yyyy-MM-dd"), request.Detail);

            conn.Query(SQLQueryGeneral);
        }

        // Get vacation state log list.
        public async Task<List<VacationStateLogDataResponse>> VacationStateLogList(VacationStateLogListRequest? request)
        {
            List<VacationStateLogDataResponse> response = new List<VacationStateLogDataResponse>();

            string SQLQueryGeneral = "SELECT U.Id AS IdUser, CONCAT(U.Name, ' ', U.Surnames) AS 'UserName', PT.Id AS 'IdPetitionType', " +
                "PT.Value AS 'ValuePetitionType', PS.Id AS 'IdPetitionState', PS.Value AS 'ValuePetitionState', V.PetitionDate AS 'PetitionDate', " +
                "StateDate, Detail FROM vacation_state_log INNER JOIN vacation V ON IdVacation = V.Id INNER JOIN `user` U ON IdUserState = U.Id " +
                "INNER JOIN petition_type PT ON V.IdPetitionType = PT.Id INNER JOIN petition_state PS ON IdState = PS.Id";
            if (request != null && request.IdUser > 0)
                SQLQueryGeneral += " AND U.Id={0}";
            if (request != null && request.IdPetitionType > 0)
                SQLQueryGeneral += " AND PT.Id={1}";
            if (request != null && request.IdPetitionState > 0)
                SQLQueryGeneral += " AND PS.Id={2}";
            if (request != null && request.PetitionDate.ToString() != "" && request.PetitionDate > new DateTime())
                SQLQueryGeneral += " AND V.PetitionDate='{3}'";
            if (request != null && request.StateDate.ToString() != "" && request.StateDate > new DateTime())
                SQLQueryGeneral += " AND StateDate='{4}'";

            string SQLQuery = String.Format(SQLQueryGeneral, request.IdUser, request.IdPetitionType, request.IdPetitionState, request.PetitionDate.Date.ToString("yyyy-MM-dd"), request.StateDate.Date.ToString("yyyy-MM-dd"));

            var SQLResponse = (await conn.QueryAsync(SQLQuery)).ToList();

            foreach (var log in SQLResponse)
            {
                VacationStateLogDataResponse logResponse = new VacationStateLogDataResponse();
                logResponse.User = new IdValue() { Id = log.IdUser, Value = log.UserName };
                logResponse.PetitionType = new IdValue() { Id = log.IdPetitionType, Value = log.ValuePetitionType };
                logResponse.PetitionState = new IdValue() { Id = log.IdPetitionState, Value = log.ValuePetitionState };
                logResponse.PetitionDate = log.PetitionDate;
                logResponse.StateDate = log.StateDate;
                logResponse.Detail = log.Detail;

                response.Add(logResponse);
            }

            return response;
        }

        // Delete vacation
        internal async Task Delete(int IdVacation)
        {
            string SQLQueryGeneral = "SELECT `Id` FROM vacation WHERE `Id` = {0};";

            string SQLQuery = String.Format(SQLQueryGeneral, IdVacation);

            var response = await conn.QueryAsync(SQLQuery); // Check if vacation exits

            if (response.Count() != 0) // If exists we delete data.
            {
                // Delete from table Vacation.
                string SQLQueryDelete = String.Format("DELETE FROM vacation WHERE Id={0};", IdVacation);
                await conn.QueryAsync(SQLQueryDelete);

                // Delete from table Vacation_State_log
                SQLQueryDelete = String.Format("DELETE FROM vacation_state_log WHERE IdVacation={0};", IdVacation);
                await conn.QueryAsync(SQLQueryDelete);
            }
            else
            {
                throw new Exception(message: $"There is no data in the database matching the id {IdVacation}");
            }
        }
    }
}
