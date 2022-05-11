using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Models;

namespace WebMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Erro(int id)
        {
            var modelErro = new ErrorViewModel();
            if (id==500)
            {
                modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Titulo = "Ocorreu um erro!";
                modelErro.ErroCode = id;
            }
            else if (id == 404)
            {
                modelErro.Mensagem= "A página que está procurando não existe! <br/> Em caso de duvida entre em contato com nosso suporte" ;

            }
            else if (id==403)
            {

            }
            else
            {
                return StatusCode(404);
            }
            return View(modelErro);
        }


        public class ResponseResult
        {
            public string Title { get; set; }
            public int Status { get; set; }
            public ResponseErrorMessages Errors { get; set; }
        }

        public class ResponseErrorMessages
        {
            public List<string> Mensagens { get; set; }
        }
    }
}
