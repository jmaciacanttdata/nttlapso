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
        public async Task<CategoriesResponse> SetCategories(SetCategoriesRequest categoriesData)
        {
            string SQLQuery = "INSERT INTO Category (Value) VALUES ("+ categoriesData.Value +")";
            int resp = conn.Execute(SQLQuery);
            CategoriesResponse response = new CategoriesResponse();
            if (resp > 0)
            {
                response.isSuccess = true;
                return response;
            }
            else
            {
                response.isSuccess = false;
                return response;
            };
        }
        public async Task<CategoriesDataResponse?> GetCategories(CategoriesRequest categoriesRequest)
        {
            CategoriesDataResponse resp = new CategoriesDataResponse();
            string SQLQuery = "SELECT Id, Value FROM Category WHERE Id '" + categoriesRequest.IdCategory + "'";
            resp = conn.Query<CategoriesDataResponse>(SQLQuery).FirstOrDefault();
            return resp;
        }
        public async Task<CategoriesResponse> UpdateCategories(CategoriesRequest categoriesData)
        {
            string SQLQuery = "UPDATE Category SET Value = `"+categoriesData.Value+ "` WHERE Id = "+categoriesData.IdCategory+"";
            int resp = conn.Execute(SQLQuery);
            CategoriesResponse response = new CategoriesResponse();
            if (resp > 0)
            {
                response.isSuccess = true;
                return response;
            }
            else
            {
                response.isSuccess = false;
                return response;
            }; ;
        }
        public async Task<CategoriesResponse> DeleteCategories(CategoriesRequest categoriesRequest)
        {
            string SQLQuery = "DELETE FROM Category WHERE Id '" + categoriesRequest.IdCategory + "'";
            int resp = conn.Execute(SQLQuery);
            CategoriesResponse response = new CategoriesResponse();
            if (resp > 0)
            {
                response.isSuccess = true;
                return response;
            }
            else
            {
                response.isSuccess = false;
                return response;
            };
        }
    }
}
