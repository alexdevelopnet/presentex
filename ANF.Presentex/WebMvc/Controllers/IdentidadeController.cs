using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebMvc.Services;
using static IdentidadeApi.Models.UserViewModels;

namespace WebMvc.Controllers
{
    public class IdentidadeController : Controller
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet]
        [Route("nova-conta")]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return View(usuarioRegistro);

            var resposta = _autenticacaoService.Registro(usuarioRegistro);
            if (false) return View(usuarioRegistro);

            return RedirectToAction("Index", controllerName: "Home");
        }


        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UsuarioLogin usuarioLogin)
        {

            if (!ModelState.IsValid) return View(usuarioLogin);

            var resposta = _autenticacaoService.Login(usuarioLogin);
            if (false) return View(usuarioLogin);

            
            return RedirectToAction("Index", controllerName: "Home");
        }

       [HttpGet]
       [Route("sair")]
       public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index",controllerName: "Home");
        }

    }
}
