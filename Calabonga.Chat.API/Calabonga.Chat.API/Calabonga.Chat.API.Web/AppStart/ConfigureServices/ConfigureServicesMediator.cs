using Calabonga.AspNetCore.Controllers.Extensions;
using Calabonga.Chat.API.Web.Mediator.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Chat.API.Web.AppStart.ConfigureServices
{
    /// <summary>
    /// ASP.NET Core services registration and configurations
    /// </summary>
    public static class ConfigureServicesMediator
    {
        /// <summary>
        /// ConfigureServices Services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddCommandAndQueries(typeof(Startup).Assembly);
        }
    }
}