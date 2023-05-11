using Dapper;
using MySqlConnector;
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
            requestLog.IdVacation = conn.ExecuteScalar<int>("SELECT id FROM vacation WHERE IdUserPetition = "+ request.IdUserPetition + " AND PetititonDate = '" + request.Day.ToString("yyyy-MM-dd") +"'");
            requestLog.IdUserState = request.IdUserPetition;
            requestLog.IdState = 1;
            requestLog.StateDate = request.Day;
            requestLog.Detail = "";
            CreateLog(requestLog);
        }

        public async Task<bool> CheckViability(int IdUserPetition, DateTime datePetition)
        {
            int result;
            DateTime date = new DateTime();
            date = datePetition.Date;
            bool teamViability = true;
            string fecha = date.ToString("yyyy-MM-dd");
            string checkIfIsSelected = "SELECT IdUser FROM user_team INNER JOIN vacation ON IdUser =IdUserPetition WHERE petitionDate = '" + fecha + "'";
            List<int> idUsers = conn.Query<int>(checkIfIsSelected).ToList();
            foreach (int idUser in idUsers)
            {
                //if user have aproved this date then it will be not available 
                if (idUser == IdUserPetition)
                {
                    teamViability = false;
                }
            }
            var resulta = conn.Query(string.Format("CALL SP_GET_VACATIONS_VIABILITY({0},'{1}')", IdUserPetition, fecha));
            if (resulta.Count() > 0)
            {
                teamViability = false;
            }
            return teamViability;
        }

        public async Task Edit(EditVacationRequest request)
        {
            EditLogRequest requestLog = new EditLogRequest();
            string query = "SELECT Id FROM vacation WHERE IdUserPetition = " + request.IdUserPetition + " AND PetitionDate = '" + request.OldPetitionDate.Date.ToString("yyyy-MM-dd") + "'";
            requestLog.IdVacation = conn.ExecuteScalar<int>(query);
            requestLog.IdUserState = request.IdUserPetition;
            requestLog.IdState = 1;
            requestLog.OldPetitionDate = request.OldPetitionDate;
            requestLog.StateDate = request.PetitionDate;
            requestLog.Detail = "";
            EditLog(requestLog);

            VacationResponse response = new VacationResponse();
            string SQLQuery = String.Format("UPDATE vacation SET PetitionDate = '{2}', IdPetitionType = {3}  WHERE IdUserPetition = {0} AND PetitionDate = '{1}'",request.IdUserPetition, request.OldPetitionDate.Date.ToString("yyyy-MM-dd"), request.PetitionDate.Date.ToString("yyyy-MM-dd"),request.IdPetitionType);
            conn.Query(SQLQuery);

        }

        public async Task VacationApproved(VacationApprovedRequest request)
        {
            CreateLogRequest requestLog = new CreateLogRequest();
            string query = "SELECT Id FROM vacation WHERE IdUserPetition = " + request.IdUserState + " AND PetitionDate = '" + request.StateDate.Date.ToString("yyyy-MM-dd") + "'";
            int IdVacation = conn.ExecuteScalar<int>(query);
            requestLog.IdVacation = IdVacation;
            requestLog.StateDate = request.StateDate;
            requestLog.IdUserState = request.IdUserState;
            requestLog.IdState = request.IdPetitionState;
            string SQLQuery = String.Format("INSERT INTO vacation_state_log (`IdVacation`, `IdUserState`, `IdState`, `StateDate`) VALUES ({0}, {1}, {2}, '{3}')", IdVacation,requestLog.IdUserState,request.IdPetitionState, requestLog.StateDate.Date.ToString("yyyy-MM-dd"));
            conn.Query(SQLQuery);
        }
        public async Task CreateLog(CreateLogRequest request)
        {
            string SQLQuery = "INSERT INTO vacation_state_log (`IdVacation`, `IdUserState`, `IdState`, `StateDate`, `Detail`) VALUES ({0}, {1}, {2}, '{3}', '{4}')";
            string SQLQueryGeneral = String.Format(SQLQuery, request.IdVacation, request.IdUserState, request.IdState,request.StateDate.ToString("yyyy-MM-dd"), request.Detail);

            conn.Query(SQLQueryGeneral);
        }

        public async Task EditLog(EditLogRequest request)
        {

            string SQLQuery = "UPDATE vacation_state_log SET StateDate = '{2}' WHERE IdVacation = {0} AND StateDate = '{1}'";
            string SQLQueryGeneral = String.Format(SQLQuery, request.IdVacation, request.OldPetitionDate?.Date.ToString("yyyy-MM-dd"), request.StateDate?.ToString("yyyy-MM-dd"));
            conn.Query(SQLQueryGeneral);
        }

    }

}
