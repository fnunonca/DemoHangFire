using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJtZXJjaGFudENvZGUiOiI0MDAwMDExIiwiZmFjaWxpdGF0b3JDb2RlIjoiMCIsInRyYW5zYWN0aW9uSWQiOiIxNzA3NDEzMzg0MjA1MDAiLCJPcmRlck51bWJlciI6Ik9OVEVTVDE3MDc0MTMzOCIsIkFtb3VudCI6IjExNjYuNjciLCJUb2tlbklkIjoiNjNlOTQ5MWMtYjkzNi00ZjY0LThlOTEtMzRlZWYyODVhMGNmIiwibmJmIjoxNzA3NDEzMzg3LCJleHAiOjE3MDc0MTQyODcsImlhdCI6MTcwNzQxMzM4N30.3gWFI5amiqPUgusA2uNfcAjt0bNAaX4VlTZ5F8N7JBM";
            string nameClaim = "merchantCode"; 

            var valueClaim = await GetClaimToken(token, nameClaim);
            Console.WriteLine($"Valor del claim '{nameClaim}': {valueClaim}");
        }

        private static async Task<string> GetClaimToken(string token, string nameClaim)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var valueClaim = tokenS.Claims.First(claim => claim.Type == nameClaim).Value;
            return await Task.FromResult(valueClaim);
        }
    }
}
