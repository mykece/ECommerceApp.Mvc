using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ETicaret.UI.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class HomeController : AdminBaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
