using System.ComponentModel.DataAnnotations;

namespace ezpeletaNetCore8.Models
{

    public class TipoEjercicio{
        
        [Key]
        public int TipoEjercicioID { get; set; }

        public string? Nombre { get; set; }

        public bool Eliminado { get; set; }

        public virtual ICollection<EjercicioFisico> EjercicioFisicos { get; set; }
        
    }

}