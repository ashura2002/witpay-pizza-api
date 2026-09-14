using Microsoft.EntityFrameworkCore;
using WitpayPizzaApi.Models;

namespace WitpayPizzaApi.Data
{
    public sealed class WitpayDbContext : DbContext
    {
        public WitpayDbContext(DbContextOptions<WitpayDbContext> options) : base(options)
        {
        }

        public DbSet<Topping> Toppings => Set<Topping>();
        public DbSet<Pizza> Pizzas => Set<Pizza>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pizza>()
                .HasMany(p => p.Toppings)
                .WithMany(t => t.Pizzas);

            modelBuilder.Entity<Pizza>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Topping>()
               .HasIndex(t => t.Name)
               .IsUnique();
        }

    }
}