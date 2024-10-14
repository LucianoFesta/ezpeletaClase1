using System.ComponentModel.DataAnnotations;

namespace ezpeletaNetCore8.Models;

public class EventoDeportivo
{
    [Key]
    public int EventoID { get; set; }
    public string Descripcion { get; set; }
    public bool Eliminado { get; set; }

    public virtual ICollection<EjercicioFisico> EjercicioFisicos { get; set; }
}