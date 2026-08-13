using CleanArchWithVerticalSliceArchTemplate.Domain.Entities;
using CleanArchWithVerticalSliceArchTemplate.Infrastructure.Database.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CleanArchWithVerticalSliceArchTemplate.Infrastructure.Database
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; } = null!;

        // Method:
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyGlobalConventions();
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}