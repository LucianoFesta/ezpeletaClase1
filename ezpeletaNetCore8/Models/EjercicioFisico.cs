using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ezpeletaNetCore8.Models;

namespace ezpeletaNetCore8.Models
{
    public class EjercicioFisico
    {
        [Key]
        public int EjercicioFisicoID { get; set; }
        public int TipoEjercicioID { get; set; }
        public int LugarID { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        [NotMapped]
        public TimeSpan IntervaloDeTiempoEjercicio { get{ return Fin - Inicio; } } //ESTE CAMPO NO SE SE ALAMACENA EN DB
        public EstadoEmocional EstadoEmocionalInicio {get; set; } 
        public EstadoEmocional EstadoEmocionalFin {get; set; } 
        public string? Observaciones {get; set; }

        public virtual TipoEjercicio TipoEjercicio { get; set; }
        public virtual Lugar Lugar { get; set; }
    }

    public class EjercicioFisicoMostrar{

        public int EjercicioFisicoID { get; set; }
        public int TipoEjercicioID { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
        public string EstadoEmocionalInicio {get; set; } 
        public string EstadoEmocionalFin {get; set; } 
        public TimeSpan IntervaloDeTiempoEjercicio { get{ return Fin - Inicio; } }
        public EstadoEmocional EstadoEmocionalInicioInt {get; set; } 
        public EstadoEmocional EstadoEmocionalFinInt {get; set; } 
        public string Lugar {get; set; }
        public string? Observaciones {get; set; }
        public string NombreTipoEjercicio { get; set; }
        public string NombreLugar { get; set; }

    }

    public enum EstadoEmocional{
        Feliz = 1,
        Triste,
        Enojado,
        Ansioso,
        Estresado,
        Relajado,
        Aburrido,
        Emocionado,
        Agobiado,
        Confundido,
        Optimista,
        Pesimista,
        Motivado,
        Cansado,
        Eufórico,
        Agitado,
        Satisfecho,
        Desanimado
    }

    public class VistaSumaEjercicioFisico
    {
        public string? TipoEjercicioNombre {get; set;}
        public int TotalidadMinutos {get; set; }
        public int TotalidadDiasConEjercicio {get;set;}
        public int TotalidadDiasSinEjercicio {get;set;}

        public List<VistaEjercicioFisico>? DiasEjercicios {get;set;}
    }

    public class VistaEjercicioFisico
    {   
        public int Anio {get; set; }  
        public string? Mes { get; set; }
        public int? Dia { get; set; }
        public int CantidadMinutos { get; set; }
    }
}