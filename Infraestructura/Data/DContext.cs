
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace OpinionesClientes.Data
{
    public class DContext : DbContext
    {
        public DbSet<WebReviews> WebReviews { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<SocialComments> SocialComments { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Survey> Survey { get; set; }
        public DbSet<FuenteDatos> FuenteDatos { get; set; }
        public DbSet<ClasificacionOpinion> ClasificacionOpinion { get; set; }
        public DbSet<TipoFuente> TipoFuente { get; set; }






        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
     @"Server=DESKTOP-MLNOSVV;Database=OpinionesClientesDB;Trusted_Connection=True;TrustServerCertificate=True;");

        }

    }
}
