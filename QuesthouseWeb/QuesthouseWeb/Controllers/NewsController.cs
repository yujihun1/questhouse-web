using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuesthouseWeb.DataContext;
using QuesthouseWeb.Models;
using QuesthouseWeb.Models.ViewModels;

public class NewsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    public NewsController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _context.NewsItems
                          .Include(n => n.Photo)
                          .OrderByDescending(n => n.Created)
                          .ToListAsync();

      

        return View(model);
    }

    public async Task<IActionResult> Detail(int Id)
    {
        var data = await _context.NewsItems
            .FirstOrDefaultAsync(p => p.Id.Equals(Id));

        if (data == null)
        {
            return NotFound();
        }
        return View(data);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Add()
    {
        // 1. 이메일 Claim 가져오기
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        // 2. @ 앞부분 추출
        string? emailId = null;
        if (!string.IsNullOrEmpty(email))
        {
            var atIndex = email.IndexOf('@');
            if (atIndex > 0)
            {
                emailId = email.Substring(0, atIndex);
            }
        }

        var viewModel = new NewsFormViewModel
        {
            News = new News
            {
                Created = DateTime.Now,
                Updated = DateTime.Now,
                User_id = emailId
            }
        }; 

        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add(NewsFormViewModel viewModel)
    {
        var model = viewModel.News;

        // 1) 파일 업로드 처리
                if (viewModel.Upload != null && viewModel.Upload.Length > 0)
                   {
            var uploads = Path.Combine(_env.WebRootPath, "images/news");
                        if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(viewModel.Upload.FileName)}";
            var filePath = Path.Combine(uploads, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                await viewModel.Upload.CopyToAsync(stream);
            
            var photo = new Photo { Photo_path = $"/images/news/{fileName}" };
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();
            model.PhotoId = photo.Photo_id;
        }

        // 2) 뉴스 저장
        _context.NewsItems.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("News","Admin");
    }
}