using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuesthouseWeb.DataContext;
using QuesthouseWeb.Models;
using QuesthouseWeb.Models.ViewModels;

namespace QuesthouseWeb.Controllers
{
    public class BbsController : Controller
    {
        private readonly AppDbContext _context;

        public BbsController(AppDbContext context)
        {
            _context = context;
        }

        // GET /Bbs
        public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
        {
			// 공개용으로 공지사항(NewsItems)을 모두 가져와서 뷰에 전달

      

			if (page < 1 || pageSize < 1)
			{
				return BadRequest("페이지 값이 잘못되었습니다.");
			}

			var totalItems = await _context.NewsItems.CountAsync();

			var newsItems = await _context.NewsItems
				.Include(n => n.Photo)
				.OrderByDescending(n => n.Created)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var viewModel = new NewsListViewModel
			{
				NewsItems = newsItems,
				CurrentPage = page,
				PageSize = pageSize,
				TotalItems = totalItems
			};

			return View(viewModel);
		}

        // GET /Bbs/Detail/5
        public async Task<IActionResult> Detail(int id)
        {
            // 1) 현재 게시물
            var item = await _context.NewsItems
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(n => n.Id == id);
            if (item == null)
                return NotFound();

            // 2) 이전 게시물 (현재보다 생성일이 작고, 가장 가까운 것)
            var prev = await _context.NewsItems
                                     .AsNoTracking()
                                     .Where(n => n.Created < item.Created)
                                     .OrderByDescending(n => n.Created)
                                     .FirstOrDefaultAsync();
            ViewBag.PreviousNews = prev;

            // 3) 다음 게시물 (현재보다 생성일이 크고, 가장 이른 것)
            var next = await _context.NewsItems
                                     .AsNoTracking()
                                     .Where(n => n.Created > item.Created)
                                     .OrderBy(n => n.Created)
                                     .FirstOrDefaultAsync();
            ViewBag.NextNews = next;

            return View(item);
        }
    }
}
