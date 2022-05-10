using IdentidadeApi.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

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
            var response = await httpClient_.PostAsync(requestUri: "https://https://localhost:44344/api/identidade/autenticar/", loginContent);

            return JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> Registro(UserViewModels.UsuarioRegistro usuarioRegistro)
        {
            var registroContent = new StringContent(
                content: JsonSerializer.Serialize(usuarioRegistro),
                Encoding.UTF8, mediaType: "application/Json");
            var response = await httpClient_.PostAsync(requestUri: "https://localhost:44344/api/identidade/nova-conta/", registroContent);

            return JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync());
        }
    }
}
