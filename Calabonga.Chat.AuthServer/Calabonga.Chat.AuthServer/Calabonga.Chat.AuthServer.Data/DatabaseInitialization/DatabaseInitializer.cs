using Calabonga.Chat.AuthServer.Entities.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calabonga.Chat.AuthServer.Data.DatabaseInitialization
{
    /// <summary>
    /// Database Initializer
    /// </summary>
    public static class DatabaseInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            await using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            // Should be uncomment when using UseSqlServer() settings or any other provider.
            // This is should not be used when UseInMemoryDatabase()
            // context.Database.Migrate();

            var roles = AppData.Roles.ToArray();

            foreach (var role in roles)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                if (!context!.Roles.Any(r => r.Name == role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role, NormalizedName = role.ToUpper() });
                }
            }

            #region developer

            var user1 = UserHelper.GetUser("user1@yopmail.com");
            await UserHelper.AddUserWithRoles(context!, scope, user1, roles);

            var user2 = UserHelper.GetUser("user2@yopmail.com");
            await UserHelper.AddUserWithRoles(context!, scope, user2, roles);

            var user3 = UserHelper.GetUser("user3@yopmail.com");
            await UserHelper.AddUserWithRoles(context!, scope, user3 , roles);

            #endregion

            await context!.SaveChangesAsync();
        }
    }
}