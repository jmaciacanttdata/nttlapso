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
        public async Task<ActionResult> SetCategories(SetCategoriesRequest request)
        {
            CategoriesResponse categoriesData = await _service.SetCategories(request);
            if (categoriesData.isSuccess == true)
            {
                return Ok("OKAY");
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("The server is not responding, check your connection and try again");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("The data entered does not exist or is incorrect, please review it and try again");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpPut]
        [Route("UpdateCategories")]
        public async Task<ActionResult> UpdateCategories(CategoriesRequest request)
        {
            CategoriesResponse categoriesData = await _service.UpdateCategories(request);
            if (categoriesData.isSuccess == true)
            {
                return Ok("OKAY");
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("The server is not responding, check your connection and try again");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("The data entered does not exist or is incorrect, please review it and try again");
                }
                else
                {
                    return BadRequest();
                }
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
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("The server is not responding, check your connection and try again");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("The data entered does not exist or is incorrect, please review it and try again");
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        [HttpDelete]
        [Route("DeleteCategories")]
        public async Task<ActionResult> DeleteCategories(CategoriesRequest request)
        {
            CategoriesResponse categoriesData = await _service.DeleteCategories(request);
            if (categoriesData.isSuccess == true)
            {
                return Ok("OKAY");
            }
            else
            {
                if (BadRequest().StatusCode == 500)
                {
                    return BadRequest("The server is not responding, check your connection and try again");
                }
                else if (BadRequest().StatusCode == 400)
                {
                    return BadRequest("The data entered does not exist or is incorrect, please review it and try again");
                }
                else
                {
                    return BadRequest();
                }
            }
        }
    }
}
