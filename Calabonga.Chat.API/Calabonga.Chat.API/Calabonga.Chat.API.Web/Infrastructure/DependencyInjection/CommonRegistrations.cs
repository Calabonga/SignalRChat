using Calabonga.Chat.API.Data;
using Calabonga.Chat.API.Web.Hubs;
using Calabonga.Chat.API.Web.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Chat.API.Web.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Registrations for both points: API and Scheduler
    /// </summary>
    public partial class DependencyContainer
    {
        /// <summary>
        /// Register 
        /// </summary>
        /// <param name="services"></param>
        public static void Common(IServiceCollection services)
        {
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();

            // services
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<ICacheService, CacheService>();

            // chat
            services.AddSingleton<ChatManager>();
        }
    }


}
