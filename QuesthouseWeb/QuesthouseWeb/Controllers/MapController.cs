using Microsoft.AspNetCore.Mvc;

namespace QuesthouseWeb.Controllers
{
	public class MapController : Controller
	{

		public IActionResult Index()
		{
			return View();
		}
	}
}
