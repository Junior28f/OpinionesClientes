

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Cliente
    {
        [Key]
        public String IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public virtual ICollection<WebReviews> WebReviews { get; set; } = new List<WebReviews>();
        public virtual ICollection<SocialComments> SocialComments { get; set; } = new List<SocialComments>();


        public Cliente(String idCliente, string nombre, string email)
        {
            IdCliente = idCliente;
            Nombre = nombre;
            Email = email;
        }
        public Cliente() { }
    }

}
