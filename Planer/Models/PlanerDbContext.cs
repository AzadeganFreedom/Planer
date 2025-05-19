using Microsoft.EntityFrameworkCore;
using Planer.Class;
using Planer.Models;

namespace Planer.Models
{
    public class PlanerDbContext : DbContext
    {
        public PlanerDbContext(DbContextOptions<PlanerDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Item> Items { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>()
        //        .HasMany(u => u.Lists)
        //        .WithOne(l => l.User)
        //        .HasForeignKey(l => l.UserId);
        //    modelBuilder.Entity<List>()
        //        .HasMany(l => l.Items)
        //        .WithOne(i => i.List)
        //        .HasForeignKey(i => i.ListId);
        //}
    }
}
