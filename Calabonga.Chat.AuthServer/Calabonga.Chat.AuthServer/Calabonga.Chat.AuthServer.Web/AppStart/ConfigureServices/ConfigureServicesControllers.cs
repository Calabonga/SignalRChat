using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Chat.AuthServer.Web.AppStart.ConfigureServices
{
    /// <summary>
    /// Configure controllers
    /// </summary>
    public static class ConfigureServicesControllers
    {
        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services) => services.AddControllersWithViews();
    }
}
