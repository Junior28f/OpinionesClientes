

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Survey
    {
        [Key]
        public int IdOpinion { get; set; }

        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; } = string.Empty;

        public string IdFuente { get; set; } = string.Empty;


        public string IdClasificacion { get; set; } = string.Empty;



        public string ClasificacionNombre { get; set; } = string.Empty;

        public int PuntajeSatisfaccion { get; set; }

        public string Fuente { get; set; } = string.Empty;

        public virtual ICollection<ClasificacionOpinion> ClasificacionOpinions { get; set; } = new List<ClasificacionOpinion>();
        public virtual ICollection<Products> Products { get; set; } = new List<Products>();
        public virtual ICollection<FuenteDatos> FuenteDatos { get; set; } = new List<FuenteDatos>();


        public Survey() { }

        public Survey(int idOpinion, int idCliente, int idProducto, DateTime fecha, string comentario,
                      string clasificacionNombre, String idClasificacion, int puntajeSatisfaccion,
                      string idFuente, string fuente)
        {
            IdOpinion = idOpinion;
            IdCliente = idCliente;
            IdProducto = idProducto;
            Fecha = fecha;
            Comentario = comentario;
            ClasificacionNombre = clasificacionNombre;
            IdClasificacion = idClasificacion;
            PuntajeSatisfaccion = puntajeSatisfaccion;
            IdFuente = idFuente; 
        }

        public override string ToString()
        {
            return $"{IdOpinion} | Cliente: {IdCliente} | Producto: {IdProducto} | {ClasificacionNombre} " +
                   $"({PuntajeSatisfaccion}/5) | {Fecha:yyyy-MM-dd} | {Comentario} | Fuente: {Fuente}";
        }
    }
}
