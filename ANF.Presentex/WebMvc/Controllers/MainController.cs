using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if (resposta != null && resposta.Errors.Mensagens.Any())
            {
                foreach (var mensagem in resposta.Errors.Mensagens)
                {
                    ModelState.AddModelError(key:string.Empty,errorMessage: mensagem);
                }

                return true;
            }

            return false;
        }
    }
}
