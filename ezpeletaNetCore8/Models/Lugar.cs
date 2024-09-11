using System.ComponentModel.DataAnnotations;

namespace ezpeletaNetCore8.Models;

public class Lugar
{
    [Key]
    public int LugarID { get; set; }
    public string Nombre { get; set; }

    public virtual ICollection<EjercicioFisico> EjeciciosFisicos { get; set; }
}