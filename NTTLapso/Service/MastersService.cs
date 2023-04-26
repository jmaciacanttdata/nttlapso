using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.Login;
using NTTLapso.Models.Categories;
using NTTLapso.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Service
{
    public class MastersService
    {
        private MastersRepository _repo = new MastersRepository();
        public MastersService() { }
        public async Task<CategoriesResponse> SetCategories(CategoriesRequest categoriesRequest)
        {
            return await _repo.SetCategories(categoriesRequest);
        }
        public async Task<CategoriesResponse> UpdateCategories(CategoriesRequest categoriesRequest)
        {
            return await _repo.UpdateCategories(categoriesRequest);
        }
        public async Task<CategoriesDataResponse> GetCategories(CategoriesRequest categoriesRequest)
        {
            return await _repo.GetCategories(categoriesRequest);
        }
        public async Task<CategoriesResponse> DeleteCategories(CategoriesRequest categoriesRequest)
        {
            return await _repo.DeleteCategories(categoriesRequest);
        }
    }
}
