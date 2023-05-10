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

    }

}
