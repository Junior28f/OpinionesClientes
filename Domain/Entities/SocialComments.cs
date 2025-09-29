

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class SocialComments
    {
        [Key]
        public string IdComment { get; set; } = string.Empty;
        public string? IdCliente { get; set; }   
        public string IdProducto { get; set; } = string.Empty;
        public string Fuente { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; } = string.Empty;


        public SocialComments() { }

 
        public SocialComments(string idComment, string? idCliente, string idProducto, string fuente, DateTime fecha, string comentario)
        {
            IdComment = idComment;
            IdCliente = idCliente;
            IdProducto = idProducto;
            Fuente = fuente;
            Fecha = fecha;
            Comentario = comentario;
        }

        public override string ToString()
        {
            return $"{IdComment} | Cliente: {(IdCliente ?? "N/A")} | Producto: {IdProducto} | {Fuente} | {Fecha:yyyy-MM-dd} | {Comentario}";
        }
    }
}
