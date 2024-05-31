namespace ezpeletaNetCore8.Models;

public class EjerciciosXdia
{
    public int Dia { get; set; }
    public string Mes { get; set; }
    public int CantidadMinutos { get; set; }
}

public class VistaTipoDeEjercicio
{
    public int TipoEjercicioID { get; set; }
    public string Descripcion { get; set; }
    public decimal CantidadMinutos { get; set; }
}
