using Dapper;
using MySqlConnector;
using NTTLapso.Models.Login;

namespace NTTLapso.Repository
{
    public class AuthRepository
    {
        private static string connectionString;
        private MySqlConnection conn;
        private IConfiguration _config;

        public AuthRepository(IConfiguration config) {
            _config = config;
            connectionString = _config.GetValue<string>("ConnectionStrings:Develop");
            conn = new MySqlConnection(connectionString);
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest) {
            LoginResponse response = new LoginResponse();
            string SQLQuery = "SELECT Id as IdUsuario, Name as Nombre, Surnames as Apellidos, Email, IDCategory as IdCategoria, IdUserSchedule as IdUsuarioHorario FROM user WHERE UserName='" + loginRequest.UserName + "' AND UserPass=MD5('" + loginRequest.Password + "') AND Active=1;";
            response = conn.Query<LoginResponse>(SQLQuery).FirstOrDefault();
            return response;
        }
    }
}
