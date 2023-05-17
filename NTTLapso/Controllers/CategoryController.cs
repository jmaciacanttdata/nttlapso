using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Category;
using NTTLapso.Models.General;
using NTTLapso.Models.Category;
using NTTLapso.Models.Login;
using NTTLapso.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("NTTLapso/Category")]
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CategoryController> _logger;
        private CategoryService _service = new CategoryService();
        public CategoryController(ILogger<CategoryController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }


        [HttpPost]
        [Route("List")]
        [Authorize]
        public async Task<ListCategoryResponse> List(IdValue? request) {
            ListCategoryResponse response = new ListCategoryResponse();
            List<IdValue> responseList = new List<IdValue>();
            try
            {
                responseList = await _service.List(request);
                response.IsSuccess = true;
                response.Data = responseList;
                response.Error = null;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;
            }

            return response;
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<CategoryResponse> Create([FromBody] string value)
        {
            CategoryResponse response = new CategoryResponse();

            try
            {
                if (value != null && value != "")
                {
                    await _service.Create(value);
                    response.IsSuccess = true;
                }
                else {
                    Error _error = new Error("El valor de categoria no puede ser nulo",null);
                    response.IsSuccess = false;
                    response.Error = _error;
                }
            }catch(Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize]
        public async Task<CategoryResponse> Edit(IdValue request)
        {
            CategoryResponse response = new CategoryResponse();

            try
            {
                await _service.Edit(request);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }

        [HttpGet]
        [Route("Delete")]
        [Authorize]
        public async Task<CategoryResponse> Delete(int Id)
        {
            CategoryResponse response = new CategoryResponse();

            try
            {
                await _service.Delete(Id);
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                Error _error = new Error(ex);
                response.IsSuccess = false;
                response.Error = _error;

            }

            return response;
        }
    }
}
