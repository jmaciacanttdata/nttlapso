using Dapper;
using MySqlConnector;
using NTTLapso.Models.Login;

namespace NTTLapso.Repository
{
    public class AuthRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public AuthRepository() { 
            conn = new MySqlConnection(connectionString);
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest) {
            LoginResponse response = new LoginResponse();
            string SQLQuery = "SELECT Id as IdUsuario, Nombre, Apellidos, Email, IdCategoria, IdUsuarioHorario FROM usuario WHERE UserName='" + loginRequest.UserName + "' AND UserPass=MD5('" + loginRequest.Password + "') AND Activo=1;";
            response = conn.Query<LoginResponse>(SQLQuery).FirstOrDefault();
            return response;
        }
    }
}
