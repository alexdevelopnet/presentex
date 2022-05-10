using IdentidadeApi.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebMvc.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly HttpClient httpClient_;

        public AutenticacaoService(HttpClient httpClient)
        {
            this.httpClient_ = httpClient;
        }

        public async Task<string> Login(UserViewModels.UsuarioLogin usuarioLogin)
        {
            var loginContent = new StringContent(
                content: JsonSerializer.Serialize(usuarioLogin),
                Encoding.UTF8, mediaType: "application/Json");
            var response = await httpClient_.PostAsync(requestUri: "https://localhost:44344/api/identidade/autenticar/", loginContent);

            var teste = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> Registro(UserViewModels.UsuarioRegistro usuarioRegistro)
        {
            var registroContent = new StringContent(
                content: JsonSerializer.Serialize(usuarioRegistro),
                Encoding.UTF8, mediaType: "application/Json");
            var response = await httpClient_.PostAsync(requestUri: "https://localhost:44344/api/identidade/nova-conta/", registroContent);

            var teste = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync());
        }
    }
}
