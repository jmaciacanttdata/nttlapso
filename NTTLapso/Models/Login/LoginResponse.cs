namespace NTTLapso.Models.Login
{
    public class LoginResponse
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public int IdCategoria { get; set; }
        public int IdUsuarioHorario { get; set; }
        public int Activo { get; set; }
        public List<LoginUserPermissionResponse>? Permission { get; set; }
    }
}
