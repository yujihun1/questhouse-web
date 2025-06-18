using Microsoft.AspNetCore.Mvc;

namespace QuesthouseWeb.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        [Route("api/upload")]
        public async Task <IActionResult> ImageUpload(IFormFile file)
        {
            // 이미지나 파일을 업로드할때 필요한 구성
            // 1. path 경로 구성 - 어디에 저장할지
            var path = Path.Combine(_environment.WebRootPath, @"images\upload");

            // 2. name 이름 구성 - 중복되지 않도록 datetime, guid 등을 활용
            // 3. extension 확장자 확인 - 이미지 파일인지 확인
            var fileName = $"{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                // 4. 파일을 저장
                await file.CopyToAsync(fileStream);
            }

            return Ok(new { file = "/images/upload/" + fileName, success = true });

        }
    }
}
