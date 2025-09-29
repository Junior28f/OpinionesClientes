using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class WebReviews
    {
        [Key]
        public string IdReview { get; set; } = string.Empty;   
        public string IdCliente { get; set; } = string.Empty;  
        public string IdProducto { get; set; } = string.Empty; 
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; } = string.Empty;
        public int Rating { get; set; } 

        public WebReviews() { }

       
        public WebReviews(string idReview, string idCliente, string idProducto, DateTime fecha, string comentario, int rating)
        {
            IdReview = idReview;
            IdCliente = idCliente;
            IdProducto = idProducto;
            Fecha = fecha;
            Comentario = comentario;
            Rating = rating;
        }
        public override string ToString()
        {
            return $"{IdReview} | Cliente: {IdCliente} | Producto: {IdProducto} | Rating: {Rating}/5 | {Fecha:yyyy-MM-dd} | {Comentario}";
        }
    }
}
