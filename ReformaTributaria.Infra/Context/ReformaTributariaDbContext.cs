using Microsoft.EntityFrameworkCore;
using ReformaTributaria.Domain.Entities;
using ReformaTributaria.Infra.Mappings;

namespace ReformaTributaria.Infra.Context
{
    public class ReformaTributariaDbContext(DbContextOptions<ReformaTributariaDbContext> options) : DbContext(options)
    {
        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Fila> Filas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("public");

            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        }
    }
}