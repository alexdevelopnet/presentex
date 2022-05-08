using Microsoft.AspNetCore.Mvc;

namespace IdentidadeApi.Controllers
{
    [Route("")]
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
