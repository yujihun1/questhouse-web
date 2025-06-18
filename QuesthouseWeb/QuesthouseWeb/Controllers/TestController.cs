using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuesthouseWeb.DataContext;
using QuesthouseWeb.Models;

public class TestController : Controller
{
    private readonly AppDbContext _context;

    public TestController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // LINQ 방식
        var data = await _context.Test.ToListAsync();

        // 또는 FromSqlRaw 방식
        // var data = await _context.Tests
        //     .FromSqlRaw("SELECT * FROM Tests")
        //     .ToListAsync();

        return View(data);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Test model)
    {
        _context.Test.Add(model);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}