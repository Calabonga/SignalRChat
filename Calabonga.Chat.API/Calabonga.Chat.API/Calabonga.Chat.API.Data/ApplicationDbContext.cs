using Calabonga.Chat.API.Data.Base;
using Calabonga.Chat.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Calabonga.Chat.API.Data
{
    /// <summary>
    /// Database for application
    /// </summary>
    public class ApplicationDbContext : DbContextBase<ApplicationDbContext>, IApplicationDbContext
    {
        /// <inheritdoc />
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        #region System

        public DbSet<Log> Logs { get; set; }

        #endregion
    }
}