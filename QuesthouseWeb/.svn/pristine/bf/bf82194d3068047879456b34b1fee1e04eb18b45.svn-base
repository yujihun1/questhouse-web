using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuesthouseWeb.DataContext;
using QuesthouseWeb.Models;

namespace QuesthouseWeb.Controllers
{
    //[ServiceFilter(typeof(IpCheckFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;


        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 최신 공지사항 불러오기
            var latestNews = await _context.NewsItems
                                             .Include(n => n.Photo)          // Photo 네비게이션 프로퍼티 로드
                                             .AsNoTracking()
                                             .OrderByDescending(n => n.Created)
                                             .Take(3)
                                             .ToListAsync();

            ViewBag.LatestNews = latestNews;

            // ★ 최신 제품 3건 불러오기
            var latestProducts = await _context.Product
                                      .Include(p => p.Photo)
                                      .Include(p => p.ProductClass)
                                      .AsNoTracking()
                                      .OrderByDescending(p => p.Created)
                                      .Take(3)
                                      .ToListAsync();
            ViewBag.LatestProducts = latestProducts;
            return View();
        }


        [Route("info")]
        public IActionResult Info()
        {
            // 회사 소개 페이지
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


       
    }
}
