using IdentidadeApi.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private readonly AppSettings _appSettings;


        public AuthController(SignInManager<IdentityUser> signInManager_, 
            UserManager<IdentityUser> userManager_,IOptions<AppSettings> appSettings)
        {
            this.signInManager_ = signInManager_;
            this.userManager_ = userManager_;
            this._appSettings = appSettings.Value;
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
                return Ok(await GerarJwt(usuarioRegistro.Email));
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
                return Ok(await GerarJwt(usuarioLogin.Email));
            }
            return BadRequest();
        }

        private async Task<UsuarioRespostaLogin> GerarJwt(string email)
        {
            var user = await userManager_.FindByEmailAsync(email);
            var claims = await userManager_.GetClaimsAsync(user);

            var identityClaims = await ObterClaimsUsuario(claims, user);
            var encodedToken = CodificarToken(identityClaims);

            return ObterRespostaToken(encodedToken, user, claims);
        }
        private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await userManager_.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string CodificarToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            return new UsuarioRespostaLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpiracaoHoras).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
