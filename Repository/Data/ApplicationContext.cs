using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Map;

namespace Repository.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<UrlEntity> Urls { get; set; }

        public DbSet<UrlClickEntity> UrlClick { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UrlEntity>(new UrlMap().Configure);
            modelBuilder.Entity<UrlClickEntity>(new UrlClickMap().Configure);
        }
    }
}