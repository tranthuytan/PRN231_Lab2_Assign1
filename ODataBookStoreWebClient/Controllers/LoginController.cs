using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ODataBookStore.Models;

namespace ODataBookStoreWebClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client = null;
        private string loginApi = "";
        public LoginController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            loginApi = "https://localhost:7057/api/Auth";
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            user.Name = "";
            HttpResponseMessage response = await client.PostAsJsonAsync<User>(loginApi + "/Login", user);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                TokenModel jwt = JsonConvert.DeserializeObject<TokenModel>(responseContent);
                //decode
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt.jwt);
                var claims = token.Claims;
                var role = claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;
                if (role != "Admin")
                {
                    ViewData["Message"] = "Your account doesn't have permission to access this page";
                    return View("Index");
                }
                var cookiesOptions = new CookieOptions
                {
                    //api set 30 days
                    Expires = DateTime.UtcNow.AddDays(30),
                    Secure = true,
                    HttpOnly = true,
                    Path = "/",
                };
                Response.Cookies.Append("jwt", jwt.jwt, cookiesOptions);
                return RedirectToAction("Index", "Book");
            }
            return View("Index");
        }
    }
}
