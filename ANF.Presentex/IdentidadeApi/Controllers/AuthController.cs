using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static IdentidadeApi.Models.UserViewModels;

namespace IdentidadeApi.Controllers
{
    [ApiController]
    [Route("api/identidade")]
    public class AuthController : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager_;
        private readonly UserManager<IdentityUser> userManager_;

        public AuthController(SignInManager<IdentityUser> signInManager_, UserManager<IdentityUser> userManager_)
        {
            this.signInManager_ = signInManager_;
            this.userManager_ = userManager_;
        }
        [HttpPost("nova-conta")]
        public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true
            };
            var result = await userManager_.CreateAsync(user, usuarioRegistro.Senha);
            if (result.Succeeded)
            {
                await signInManager_.SignInAsync(user, isPersistent: false);
                return Ok();
            }
            return BadRequest();
        }

     [HttpPost("autenticar")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await signInManager_.PasswordSignInAsync(userName: usuarioLogin.Email, password: usuarioLogin.Senha
                , isPersistent: false, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}