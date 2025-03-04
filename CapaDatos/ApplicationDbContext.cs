using CapaEntidad;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<PacienteCLS> PACIENTES { get; set; }

        public DbSet<MedicoCLS> MEDICOS { get; set; }
        public DbSet<CitaCLS> CITAS { get; set; }
        public DbSet<CitaViewCLS> CitasResultado { get; set; }
        public DbSet<TratamientoCLS> TRATAMIENTOS { get; set; }
        public DbSet<FacturacionCLS> FACTURACION { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CitaViewCLS>().HasNoKey().ToView("CitasResultado");
        }

    }
}
