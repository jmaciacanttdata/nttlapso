using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Service
{
    public class AuthService
    {
        private IConfiguration _config;
        private AuthRepository _repo;

        public AuthService(IConfiguration config) 
        {
            _config = config;
            _repo = new AuthRepository(_config);
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest) { 
            return await _repo.Login(loginRequest);
        }

        public async Task<string> GenerateToken(LoginResponse user, IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid,user.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name,user.Nombre),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.IdCategoria.ToString()),
                new Claim(ClaimTypes.PrimaryGroupSid,user.IdUsuarioHorario.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(System.Convert.ToInt32(_config["Jwt:TimeExpires"])),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<LoginResponse> GetCurrentUser(ClaimsIdentity identity)
        { 
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new LoginResponse
                {
                    IdUsuario = System.Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value),
                    Nombre = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    IdCategoria = System.Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value),
                    IdUsuarioHorario = System.Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimaryGroupSid)?.Value)
                };
            }
            return null;
        }
    }
}
