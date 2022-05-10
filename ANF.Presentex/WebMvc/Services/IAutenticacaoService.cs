using System.Threading.Tasks;
using static IdentidadeApi.Models.UserViewModels;

namespace WebMvc.Services
{
    public interface IAutenticacaoService
    {
        Task<string> Login(UsuarioLogin usuarioLogin);
        Task<string> Registro(UsuarioRegistro usuarioRegistro);
    }
}
