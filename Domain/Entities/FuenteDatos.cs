

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{ 
    public class FuenteDatos
    {
        [Key]
        public string IdFuente { get; set; } = string.Empty; 

        public string NombreFuente { get; set; } = string.Empty;

        public int IdTipoFuente { get; set; }

        public DateTime FechaCarga { get; set; }

        public virtual ICollection<TipoFuente> TipoFuentes { get; set; } = new List<TipoFuente>();
        


        public FuenteDatos(string idFuente, string nombreFuente, int idTipoFuente, DateTime fechaCarga)
        {
            IdFuente = idFuente;
            NombreFuente = nombreFuente;
            IdTipoFuente = idTipoFuente;
            FechaCarga = fechaCarga;
        }

        public FuenteDatos() { }
    }
}
