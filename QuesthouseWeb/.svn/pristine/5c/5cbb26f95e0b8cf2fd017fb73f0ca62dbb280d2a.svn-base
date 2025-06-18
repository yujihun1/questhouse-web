using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuesthouseWeb.DataContext;
using QuesthouseWeb.Models;
using QuesthouseWeb.Models.ViewModels;

namespace QuesthouseWeb.Controllers
{
	public class AdminController : Controller
	{
		private readonly AppDbContext _context;

		public AdminController(AppDbContext context)
		{
			_context = context;
		}

        [Authorize]
        [HttpGet]
		public async Task<IActionResult> Products(int page = 1, int pageSize = 9)
		{
			if (page < 1 || pageSize < 1)
			{
				return BadRequest("페이지 값이 잘못되었습니다.");
			}

			var totalItems = await _context.Product.CountAsync();

			if (totalItems == 0)
			{
				return View(new ProductListViewModel
				{
					Items = new List<Product>(),
					Page = page,
					PageSize = pageSize,
					TotalItems = 0
				});
			}

			var products = await _context.Product
				.Include(p => p.Photo)
				.OrderBy(p => p.Id)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			var viewModel = new ProductListViewModel
			{
				Items = products,
				Page = page,
				PageSize = pageSize,
				TotalItems = totalItems
			};

			return View(viewModel);
		}

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> News(int page = 1, int pageSize = 9)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("페이지 값이 잘못되었습니다.");
            }

            var totalItems = await _context.NewsItems.CountAsync();

            if (totalItems == 0)
            {
                return View(new NewsListViewModel
                {
                    NewsItems = new List<News>(),
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = 0
                });
            }

            var news = await _context.NewsItems
                .Include(p => p.Photo)
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var viewModel = new NewsListViewModel
            {
                NewsItems = news,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(viewModel);
        }
    }
}
