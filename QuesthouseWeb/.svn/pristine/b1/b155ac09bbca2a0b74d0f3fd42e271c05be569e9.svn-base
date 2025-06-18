using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace QuesthouseWeb.Controllers
{
    public class AccountController : Controller
    {
        // 로그인
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                // 이미 로그인된 상태면 홈으로 리다이렉트
                return RedirectToAction("Index", "Home");
            }

            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            }, "NaverWorks");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // 1. 서버 측 인증 쿠키 제거
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
