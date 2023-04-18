using NTTLapso.Models.Login;
using NTTLapso.Repository;

namespace NTTLapso.Service
{
    public class AuthService
    {
        private AuthRepository _repo = new AuthRepository();
        public AuthService() { }

        public async Task<LoginResponse> Login(LoginRequest loginRequest) { 
            return await _repo.Login(loginRequest);
        }
    }
}
