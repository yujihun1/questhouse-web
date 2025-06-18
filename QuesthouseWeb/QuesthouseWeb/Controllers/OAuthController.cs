using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace QuesthouseWeb.Controllers
{
    public class OAuthController : Controller
    {
        private const string clientId = "Xjzrl4sCpAJz3HGW6I4q";
        private const string clientSecret = "GzD8Uw8M1T";
        private const string redirectUri = "https://localhost:44322/signin-naverworks";
        private const string state = "random_state_123"; // CSRF 방지용

        [HttpGet("/oauth/naverworks-login")]
        public IActionResult NaverWorksLogin()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["response_type"] = "code";
            query["client_id"] = clientId;
            query["redirect_uri"] = redirectUri;
            query["scope"] = "user.read";
            query["state"] = state;

            var url = $"https://auth.worksmobile.com/oauth2/v2.0/authorize?{query}";
            return Redirect(url);
        }

        // [3단계] 로그인 후 네이버웍스에서 리디렉션될 때 실행됨
        [HttpGet("/signin-naverworks")]
        public async Task<IActionResult> Callback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Authorization code not found");

            using var client = new HttpClient();
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://auth.worksmobile.com/oauth2/v2.0/token");

            var parameters = new[]
            {
            $"grant_type=authorization_code",
            $"code={code}",
            $"client_id={clientId}",
            $"client_secret={clientSecret}",
            $"redirect_uri={redirectUri}",
        };

            tokenRequest.Content = new StringContent(string.Join("&", parameters), Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await client.SendAsync(tokenRequest);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return BadRequest("Token 요청 실패: " + content);

            var json = JsonDocument.Parse(content);
            var accessToken = json.RootElement.GetProperty("access_token").GetString();

            return RedirectToAction("UserInfo", new { accessToken });
        }

        // [4단계] 액세스 토큰으로 사용자 정보 요청
        [HttpGet("/userinfo")]
        public async Task<IActionResult> UserInfo(string accessToken)
        {
            using var client = new HttpClient(); 
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.worksapis.com/v1.0/users/me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            return Content(json, "application/json");
        }
    }
}