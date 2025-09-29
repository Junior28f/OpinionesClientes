using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("ClasificacionOpinion", Schema = "dbo")]
    public class ClasificacionOpinion
    {
        [Key]
       
        public String IdClasificacion { get; set; }

        [Column("ClasificacionNombre")]
        public string ClasificacionNombre { get; set; } = string.Empty;

        public ClasificacionOpinion() { }

        public ClasificacionOpinion(string clasificacionNombre)
        {
            ClasificacionNombre = clasificacionNombre;
        }
    }
}
