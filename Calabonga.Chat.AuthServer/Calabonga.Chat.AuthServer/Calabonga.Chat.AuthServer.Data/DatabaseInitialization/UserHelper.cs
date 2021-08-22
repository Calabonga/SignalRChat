using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Chat.AuthServer.Data.DatabaseInitialization
{
    public static class UserHelper
    {
        public static ApplicationUser GetUser(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

            return new ApplicationUser
            {
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = email,
                FirstName = "Microservice",
                LastName = "Administrator",
                NormalizedUserName = email.ToUpper(),
                PhoneNumber = "+79000000000",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ApplicationUserProfile = new ApplicationUserProfile
                {
                    CreatedAt = DateTime.Now,
                    CreatedBy = "SEED",
                    Permissions = new List<MicroservicePermission>
                    {
                        new()
                        {
                            CreatedAt = DateTime.Now,
                            CreatedBy = "SEED",
                            PolicyName = "Logs:UserRoles:View",
                            Description = "Access policy for Logs controller user view"
                        }
                    }
                }
            };
        }

        public static async Task AddUserWithRoles(ApplicationDbContext context, IServiceScope scope, ApplicationUser user, string[] roles)
        {
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "123qwe!@#");
                user.PasswordHash = hashed;
                var userStore = scope.ServiceProvider.GetService<ApplicationUserStore>();
                var result = await userStore!.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Cannot create account");
                }

                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                foreach (var role in roles)
                {
                    var roleAdded = await userManager!.AddToRoleAsync(user, role);
                    if (roleAdded.Succeeded)
                    {
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
