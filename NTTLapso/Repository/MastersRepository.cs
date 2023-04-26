using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using NTTLapso.Models.Categories;
using NTTLapso.Models.Login;

namespace NTTLapso.Repository
{
    public class MastersRepository
    {
        private static string connectionString = "Server=POAPMYSQL143.dns-servicio.com;User ID=nttlapso;Password=kP0?8u50a;Database=8649628_nttlapso";
        private MySqlConnection conn;

        public MastersRepository() { 
            conn = new MySqlConnection(connectionString);
        }
        public async Task<CategoriesResponse> SetCategories(CategoriesRequest categoriesData)
        {
            CategoriesResponse resp = new CategoriesResponse();
            string SQLQuery = "INSERT INTO Category (Id, Value) VALUES ('" + categoriesData.IdCategory + "', "+ categoriesData.Value +")";
            resp = conn.Query<CategoriesResponse>(SQLQuery).FirstOrDefault();
            return resp;
        }
        public async Task<CategoriesDataResponse> GetCategories(CategoriesRequest categoriesRequest)
        {
            CategoriesDataResponse resp = new CategoriesDataResponse();
            string SQLQuery = "SELECT Id, Value FROM Category WHERE Id '" + categoriesRequest.IdCategory + "'";
            resp = conn.Query<CategoriesDataResponse>(SQLQuery).FirstOrDefault();
            return resp;
        }
        public async Task<CategoriesResponse> UpdateCategories(CategoriesRequest categoriesData)
        {
            CategoriesResponse resp = new CategoriesResponse();
            string SQLQuery = "UPDATE Category SET Value = `"+categoriesData.Value+ "` WHERE Id = "+categoriesData.IdCategory+"";
            resp = conn.Query<CategoriesResponse>(SQLQuery).FirstOrDefault();
            return resp;
        }
        public async Task<CategoriesResponse> DeleteCategories(CategoriesRequest categoriesRequest)
        {
            CategoriesResponse resp = new CategoriesResponse();
            string SQLQuery = "DELETE FROM Category WHERE Id '" + categoriesRequest.IdCategory + "'";
            resp = conn.Query<CategoriesResponse>(SQLQuery).FirstOrDefault();
            return resp;
        }
    }
}
