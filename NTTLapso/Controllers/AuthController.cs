using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private AuthService _service = new AuthService();
        public AuthController(ILogger<AuthController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        private string GenerateToken(LoginResponse user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid,user.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name,user.Nombre),
                new Claim(ClaimTypes.Surname,user.Apellidos),
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

        private LoginResponse GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new LoginResponse
                {
                    IdUsuario = System.Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value),
                    Nombre = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    Apellidos = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value,
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    IdCategoria = System.Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value),
                    IdUsuarioHorario = System.Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimaryGroupSid)?.Value)
                };
            }
            return null;
        }


        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequest request) {
            LoginResponse response = await _service.Login(request);
            if (response != null)
            {
                var token = GenerateToken(response);
                return Ok(token);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("GetNombreUsuario")]
        [Authorize]
        public async Task<String> GetNombreUsuario(CancellationToken cancellationToken) {
            LoginResponse Data = GetCurrentUser();
            String response = "";
            response = Data.Nombre;
            if (Data.Apellidos != null)
                response += " " + Data.Apellidos;

            return response;
        }
    }
}
