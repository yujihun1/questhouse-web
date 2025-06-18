using Microsoft.AspNetCore.Mvc;
using QuesthouseWeb.DataContext;
using QuesthouseWeb.Models;

namespace QuesthouseWeb.Controllers
{
    [Route("ask")]
    public class AskController : Controller
    {
        private readonly AppDbContext _context;

        public AskController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /ask
        [HttpGet("")]
        public IActionResult Index()
        {
            // 문의 폼 뷰를 반환
            return View();
        }

        // POST: /ask
        [HttpPost("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Name,Email,Mobile,Content")] Inquiry model)
        {
            if (!ModelState.IsValid)
            {
                // 유효성 검사 실패 시 폼으로 돌아가기
                return View(model);
            }

            // Created와 AnsYn은 모델 초기에 기본값이 설정되어 있으므로, 따로 세팅하지 않아도 됨
            model.Created = DateTime.UtcNow;  
            model.AnsYn = "N";

            _context.Inquiries.Add(model);
            await _context.SaveChangesAsync();

            // 저장 후 간단한 감사 페이지(또는 리디렉션)로 이동
            return RedirectToAction(nameof(Thanks));
        }

        // GET: /ask/thanks
        [HttpGet("thanks")]
        public IActionResult Thanks()
        {
            return View();
        }
    }
}
