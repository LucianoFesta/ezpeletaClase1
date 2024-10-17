namespace ezpeletaNetCore8.Models;

public class EjerciciosPorTipo
{
    public string Tipo { get; set; }
    public List<EjercicioFisicoMostrar> Ejercicios { get; set; }
}

public class TipoEjerciciosLugar
{
    public string Lugar { get; set; }
    public List<EjerciciosPorTipo> EjerciciosTipoLugar { get; set; }
}

public class LugarEventoEjercicios
{
    public string Evento { get; set; }
    public List<TipoEjerciciosLugar> EjerciciosLugarEvento { get; set; }
}