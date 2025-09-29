using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Products
    {
        [Key]
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
      


        public Products() { }

        public Products(int idProducto, string nombre, string categoria)
        {
            IdProducto = idProducto;
            Nombre = nombre;
            Categoria = categoria;
        }

        public override string ToString()
        {
            return $"{IdProducto} - {Nombre} ({Categoria})";
        }
    }
}
