using Microsoft.AspNetCore.Mvc;

namespace Keeper.Main.Middleware
{
    public class ImageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
