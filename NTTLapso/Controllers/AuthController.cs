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
        private PermissionService _permissionService = new PermissionService();
        public AuthController(ILogger<AuthController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }


        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequest request) {
            LoginResponse userData = await _service.Login(request);
            List<LoginUserPermissionResponse> userPermissionResponse;
            if (userData != null)
            {
                var token = await _service.GenerateToken(userData, _config);
                LoginDataResponse response = new LoginDataResponse();
                response.Data = userData;
                response.DateLogin = DateTime.Now;
                response.DateLoginExpires = DateTime.Now.AddMinutes(System.Convert.ToInt32(_config["Jwt:TimeExpires"]));
                response.Token = token;

                userPermissionResponse = await _permissionService.ListUserPermission(userData.IdUsuario);
                userData.Permission = userPermissionResponse;
                response.Data = userData;
                return Ok(response);
            }
            else
            {
                return Forbid();
            }
        }

        [HttpGet]
        [Route("GetNombreUsuario")]
        [Authorize]
        public async Task<String> GetNombreUsuario(CancellationToken cancellationToken) {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            LoginResponse Data = await _service.GetCurrentUser(identity);
            String response = "";
            response = Data.Nombre;
            if (Data.Apellidos != null)
                response += " " + Data.Apellidos;

            return response;
        }
    }
}
