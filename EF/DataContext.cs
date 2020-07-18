using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EF
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        //Tables
        public DbSet<Domain.Car> Car { get; set; }
        public DbSet<Domain.RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    public static class Config
    {
        public static void DatabaseConfigOptions(DbContextOptionsBuilder options, string connection)
        {
            //Use SQL Server provider
            options.UseSqlServer(connection);
        }

        public static void ConfigureAspNetIdentity(this IServiceCollection services)
        {
            //Add built-in Asp.Net Identity tables
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<DataContext>();
        }
    }
}
