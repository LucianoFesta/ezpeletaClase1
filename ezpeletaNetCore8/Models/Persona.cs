using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ezpeletaNetCore8.Models
{
    public class Persona
    {
        [Key]
        public int PersonaID { get; set; }
        public string UsuarioID { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public Genero Genero { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Peso { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public decimal Altura { get; set; }

        public virtual ICollection<EjercicioFisico> EjerciciosFisicos { get; set; }
        public virtual ICollection<Lugar> Lugares { get; set; }
    }
}


public enum Genero
{
    Masculino = 1,
    Femenino,
    Otro
}