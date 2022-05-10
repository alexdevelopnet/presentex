using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebMvc.Extensions;
using WebMvc.Services;

namespace WebMvc.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
           services.AddScoped<IUser, AspNetUser>();
        }
    }
}
