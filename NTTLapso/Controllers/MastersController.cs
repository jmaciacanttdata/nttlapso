using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Categories;
using NTTLapso.Models.Login;
using NTTLapso.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("Masters")]
    public class MastersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MastersController> _logger;
        private MastersService _service = new MastersService();
        public MastersController(ILogger<MastersController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpPost]
        [Route("SetCategories")]
        public async Task<ActionResult> SetCategories(CategoriesRequest request)
        {
            CategoriesResponse categoriesData = await _service.SetCategories(request);
            CategoriesResponse response = new CategoriesResponse();
            if (categoriesData != null)
            {
                response.isSuccess = true;
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("UpdateCategories")]
        public async Task<ActionResult> UpdateCategories(CategoriesRequest request)
        {
            CategoriesResponse categoriesData = await _service.UpdateCategories(request);
            CategoriesResponse response = new CategoriesResponse();
            if (categoriesData != null)
            {
                response.isSuccess = true;
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<ActionResult> GetCategories([FromQuery]CategoriesRequest request)
        {
            CategoriesDataResponse categoriesData = await _service.GetCategories(request);
            if (categoriesData != null)
            {
                return Ok(categoriesData);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("DeleteCategories")]
        public async Task<ActionResult> DeleteCategories(CategoriesRequest request)
        {
            CategoriesResponse categoriesData = await _service.DeleteCategories(request);
            CategoriesResponse response = new CategoriesResponse();
            if (categoriesData != null)
            {
                response.isSuccess = true;
                return Ok(response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
