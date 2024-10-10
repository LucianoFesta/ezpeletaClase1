
namespace ezpeletaNetCore8.Models;

public class EjerciciosPorLugarMostrar
{
    public string TipoEjercicio { get; set; }

    public List<EjerciciosLugar> EjerciciosLugar { get; set; }
 
}

public class EjerciciosLugar
{
    public string Lugar { get; set; }
    public List<EjercicioFisicoMostrar> Ejercicios { get; set; }
}