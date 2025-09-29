

using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TipoFuente
    {
        [Key]
        public int IdTipoFuente { get; set; }

        public string NombreTipo { get; set; } = string.Empty;
    }
}
