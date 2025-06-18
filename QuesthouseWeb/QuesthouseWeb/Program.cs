using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using QuesthouseWeb.DataContext;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQLite 연결
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// IP 체크 필터 등록
//builder.Services.AddScoped<IpCheckFilter>();

// 네이버웍스 로그인 api 등록
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "NaverWorks";
})
.AddCookie()
.AddOAuth("NaverWorks", options =>
{
    options.ClientId = "Xjzrl4sCpAJz3HGW6I4q";
    options.ClientSecret = "GzD8Uw8M1T";
    options.CallbackPath = new PathString("/signin-naverworks");

    // ✅ Naver Works용 OAuth2 Endpoint
    options.AuthorizationEndpoint = "https://auth.worksmobile.com/oauth2/v2.0/authorize";
    options.TokenEndpoint = "https://auth.worksmobile.com/oauth2/v2.0/token";
    options.UserInformationEndpoint = "https://www.worksapis.com/v1.0/users/me"; // 사용자의 정보 조회

    // scope
    options.Scope.Add("user.read");

    options.SaveTokens = true;

    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.Add("consumerKey", options.ClientId); // 네이버웍스는 consumerKey를 따로 요구함

            var response = await context.Backchannel.SendAsync(request);
            response.EnsureSuccessStatusCode();

            using var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var root = user.RootElement;

            context.RunClaimActions(root);

            // ✅ 사용자 이름 구성 후 ClaimTypes.Name에 수동 매핑
            var userName = root.GetProperty("userName");
            var firstName = userName.GetProperty("firstName").GetString();
            var lastName = userName.GetProperty("lastName").GetString();

            if (!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(lastName))
            {
                var fullName = $"{lastName} {firstName}".Trim();
                context.Identity.AddClaim(new Claim(ClaimTypes.Name, fullName));
            }
        }
    };
});

// HTTP Context Accessor 등록
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
