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
        public async Task<ActionResult> SetCategories(CategoriesRequest  request)
        {
            CategoriesResponse categoriesData = await _service.RegisterCategories(request);
            CategoriesDataResponse response = new CategoriesDataResponse();
            if (categoriesData != null)
            {
                response.isSuccessful = 1;
                return Ok(response);
            }
            else
            {
                response.isSuccessful = 0;
                return Forbid();
            }
        }

        [HttpPost]
        [Route("EditCategories")]
        public async Task<ActionResult> EditCategories(CategoriesRequest request)
        {
            CategoriesResponse categoriesData = await _service.EditCategories(request);
            CategoriesDataResponse response = new CategoriesDataResponse();
            if (categoriesData != null)
            {
                response.isSuccessful = 1;
                return Ok(response);
            }
            else
            {
                response.isSuccessful = 0;
                return Forbid();
            }
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<ActionResult> GetCategories(CategoriesRequest request)
        {
            CategoriesResponse categoriesData = await _service.GetCategories(request);
            CategoriesDataResponse response = new CategoriesDataResponse();
            if (categoriesData != null)
            {
                response.Data = categoriesData;
                return Ok(response);
            }
            else
            {
                return Forbid();
            }
        }

        [HttpDelete]
        [Route("DeleteCategories")]
        public async Task<ActionResult> DeleteCategories(CategoriesRequest request)
        {
            CategoriesResponse categoriesData = await _service.DeleteCategories(request);
            CategoriesDataResponse response = new CategoriesDataResponse();
            if (categoriesData != null)
            {
                response.isSuccessful = 1;
                return Ok(response);
            }
            else
            {
                response.isSuccessful = 0;
                return Forbid();
            }
        }
    }
}
