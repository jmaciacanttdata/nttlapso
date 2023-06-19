using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NTTLapso.Models.General;
using NTTLapso.Models.Login;
using NTTLapso.Models.PetitionStatus;
using NTTLapso.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NTTLapso.Service
{
    public class CategoryService
    {
        private CategoryRepository _repo;
        private IConfiguration _configuration;
        public CategoryService(IConfiguration config) 
        {
            _configuration = config;
            _repo = new CategoryRepository(_configuration);
        }

        public async Task<List<IdValue>> List(IdValue request) {
            return await _repo.List(request);
        }

        public async Task Create(string value)
        {
            await _repo.Create(value);
        }

        public async Task Edit(IdValue request)
        {
            await _repo.Edit(request);
        }

        public async Task Delete(int Id)
        {
            await _repo.Delete(Id);
        }
    }

}
