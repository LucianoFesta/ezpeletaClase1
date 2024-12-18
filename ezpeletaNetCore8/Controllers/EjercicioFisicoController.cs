using Microsoft.AspNetCore.Mvc;
using ezpeletaNetCore8.Models;
using ezpeletaNetCore8.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Identity;

namespace ezpeletaNetCore8.Controllers;

[Authorize]
public class EjercicioFisicoController : Controller
{
    private ApplicationDbContext _context; //inicializamos el contexto
    private readonly UserManager<IdentityUser> _userManager;

    //CONTRUCTOR de la clase para traer los datos del contexto (base de datos)
    public EjercicioFisicoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;

        _userManager = userManager;
    }


    public IActionResult Reporte()
    {
        return View();
    }

public JsonResult ListadoReporte(DateTime? Desde, DateTime? Hasta)
{

    var userID = _userManager.GetUserId(User);
    var persona = _context.Personas.Where(p => p.UsuarioID == userID).SingleOrDefault();
    var listado = new List<EjercicioFisico>();

    if(User.IsInRole("Administrador"))
    {
        listado = _context.EjerciciosFisicos
            .Include(e => e.TipoEjercicio)
            .Include(e => e.Lugar)
            .Include(e => e.EventoDeportivo)
            .ToList();
    }else
    {
        listado = _context.EjerciciosFisicos
            .Include(e => e.TipoEjercicio)
            .Include(e => e.Lugar)
            .Include(e => e.EventoDeportivo)
            .Where(e => e.PersonaID == persona.PersonaID)
            .ToList();
    }


    if (Desde != null && Hasta != null)
    {
        listado = listado.Where(e => e.Inicio >= Desde && e.Fin <= Hasta).ToList();
    }

    if (listado.Count > 0)
    {
        var newListado = listado.Select(e => new EjercicioFisicoMostrar
        {
            EjercicioFisicoID = e.EjercicioFisicoID,
            Inicio = e.Inicio,
            Fin = e.Fin,
            EstadoEmocionalInicio = e.EstadoEmocionalInicio.ToString(),
            EstadoEmocionalFin = e.EstadoEmocionalFin.ToString(),
            EstadoEmocionalInicioInt = e.EstadoEmocionalInicio,
            EstadoEmocionalFinInt = e.EstadoEmocionalFin,
            Observaciones = e.Observaciones,
            NombreTipoEjercicio = e.TipoEjercicio.Nombre,
            NombreLugar = e.Lugar.Nombre,
            NombreEvento = e.EventoDeportivo.Descripcion
        })
        .OrderBy(e => e.Inicio)
        .OrderBy(e => e.NombreLugar)
        .OrderBy(e => e.NombreTipoEjercicio)
        .OrderBy(e => e.NombreEvento)
        .GroupBy(e => e.NombreEvento);

        var ejerciciosPorEvento = new List<LugarEventoEjercicios>();

        foreach (var ejerciciosEvento in newListado)
        {
            var ejercicioEvento = new LugarEventoEjercicios(){
                Evento = ejerciciosEvento.First().NombreEvento,
                EjerciciosLugarEvento = new List<TipoEjerciciosLugar>()
            };

            var agrupoPorLugar = ejerciciosEvento.GroupBy(e => e.NombreLugar);

            foreach (var ejerciciosLugar in agrupoPorLugar)
            {
                var ejercicioLugar = new TipoEjerciciosLugar(){
                    Lugar = ejerciciosLugar.First().NombreLugar,
                    EjerciciosTipoLugar = new List<EjerciciosPorTipo>()
                };

                var agrupoPorTipoEjercicio = ejerciciosLugar.GroupBy(e => e.NombreTipoEjercicio);

                foreach (var ejerciciosTipo in agrupoPorTipoEjercicio)
                {
                    var ejercicioTipo = new EjerciciosPorTipo(){
                        Tipo = ejerciciosTipo.First().NombreTipoEjercicio,
                        Ejercicios = ejerciciosTipo.ToList()
                    };

                    ejercicioLugar.EjerciciosTipoLugar.Add(ejercicioTipo);
                }

                ejercicioEvento.EjerciciosLugarEvento.Add(ejercicioLugar);
            }

            ejerciciosPorEvento.Add(ejercicioEvento);
        }

        return Json(ejerciciosPorEvento);
    }

    return Json(true);
}


    public IActionResult Index()
    {
        var userID = _userManager.GetUserId(User);
        var persona = _context.Personas.Where(p => p.UsuarioID == userID).SingleOrDefault();

        //Crear lista de selectListItem
        var selectListItem = new List<SelectListItem>
        {
            new SelectListItem{ Value = "0", Text = "[Seleccione..]"}
        };

        //Obtener las opciones del enum
        var enumValues = Enum.GetValues(typeof(EstadoEmocional)).Cast<EstadoEmocional>();
        
        //Convertir las opciones del enum en SelectItem
        selectListItem.AddRange(enumValues.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));

        //Pasar la lista a la vista
        ViewBag.EstadoEmocionalInicio = selectListItem.OrderBy(t => t.Text).ToList();
        ViewBag.EstadoEmocionalFin = selectListItem.OrderBy(t => t.Text).ToList();


        var tipoEjercicios = _context.TipoEjercicios.Where(t => t.Eliminado == false).ToList();
        var tipoEjerciciosIDBuscar = _context.TipoEjercicios.Where(t => t.Eliminado == false).ToList();
        tipoEjercicios.Add(new TipoEjercicio
        {
            TipoEjercicioID = 0,
            Nombre = "[Seleccione...]"
        });

        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Nombre), "TipoEjercicioID", "Nombre");
        tipoEjerciciosIDBuscar.Add(new TipoEjercicio
        {
            TipoEjercicioID = 0,
            Nombre = "[BUSCAR TODOS]"
        });
        ViewBag.TipoEjercicioIDBuscar = new SelectList(tipoEjerciciosIDBuscar.OrderBy(c => c.Nombre), "TipoEjercicioID", "Nombre");
        

        //VIEWBAG PARA EL GRÁFICO
        var listadoTipoEjercicios = _context.TipoEjercicios.Where(t => t.Eliminado == false).ToList();
        ViewBag.TipoEjercicioIDGrafico = new SelectList(listadoTipoEjercicios.OrderBy(t => t.Nombre), "TipoEjercicioID", "Nombre");

        var listaLugares = new List<Lugar>();

        if(User.IsInRole("Administrador"))
        {
            listaLugares = _context.Lugares.ToList();
        }else
        {
            listaLugares = _context.Lugares
                .Where(l => l.PersonaID == persona.PersonaID)
                .ToList();
        }
        listaLugares.Add(new Lugar(){
            LugarID = 0,
            Nombre = "[Seleccione...]"
        });
        ViewBag.Lugar = new SelectList(listaLugares.OrderBy(l => l.Nombre), "LugarID", "Nombre");

        var listaEventos = _context.EventosDeportivos.Where(e => e.Eliminado == false).ToList();
        listaEventos.Add(new EventoDeportivo(){
            EventoID = 0,
            Descripcion = "[Seleccione...]"
        });
        ViewBag.Evento = new SelectList(listaEventos.OrderBy(e => e.Descripcion), "EventoID", "Descripcion");

        return View();

    }

    
    public JsonResult ListadoEjerciciosFisicos(int? id, DateTime? FechaDesdeBuscar, DateTime? FechaHastaBuscar, int? TipoEjercicioFisicoID)
    {
        var userID = _userManager.GetUserId(User);
        var persona = _context.Personas.Where(p => p.UsuarioID == userID).SingleOrDefault();

        var ejerciciosFisicos = new List<EjercicioFisico>();

        if(User.IsInRole("Deportista"))
        {
            ejerciciosFisicos = _context.EjerciciosFisicos
                .Include(e => e.TipoEjercicio)
                .Include(e => e.Lugar)
                .Include(e => e.Persona)
                .Where(e => e.PersonaID == persona.PersonaID)
                .ToList();
        }else{
            ejerciciosFisicos = _context.EjerciciosFisicos
                .Include(e => e.TipoEjercicio)
                .Include(e => e.Lugar)
                .Include(e => e.Persona)
                .ToList();
        }

        if (id.HasValue){   
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.EjercicioFisicoID == id).ToList();
        
        }

        if(FechaDesdeBuscar != null && FechaHastaBuscar != null){
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.Inicio >= FechaDesdeBuscar && e.Fin <= FechaHastaBuscar).ToList();
        }

        if(TipoEjercicioFisicoID != 0 && TipoEjercicioFisicoID != null){
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.TipoEjercicioID == TipoEjercicioFisicoID).ToList();
        }

        var newListEjerciciosFisicos = ejerciciosFisicos.Select(e => new EjercicioFisicoMostrar()
        {
            EjercicioFisicoID = e.EjercicioFisicoID,
            Inicio = e.Inicio,
            Fin = e.Fin,
            EstadoEmocionalInicio = e.EstadoEmocionalInicio.ToString(),
            EstadoEmocionalFin = e.EstadoEmocionalFin.ToString(),
            EstadoEmocionalInicioInt = e.EstadoEmocionalInicio,
            EstadoEmocionalFinInt = e.EstadoEmocionalFin,
            Observaciones = e.Observaciones,
            Lugar = e.Lugar.Nombre,
            NombreTipoEjercicio = e.TipoEjercicio.Nombre
            
        }).ToList();

        return Json(newListEjerciciosFisicos);
    }

    public JsonResult SaveEjercicio(int ejercicioFisicoID, int tipoEjercicioID, DateTime inicio, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, DateTime fin, int lugar, string observaciones, int evento){

        if(tipoEjercicioID <= 0 || inicio == DateTime.MinValue || estadoEmocionalInicio == 0 || estadoEmocionalFin == 0 || fin == DateTime.MinValue || string.IsNullOrEmpty(observaciones) || lugar <= 0 || evento <= 0)
        {
            return Json(new { success = false, message = "Por favor, completa todos los campos para poder crear un ejercicio físico." });

        }else{

            if(ejercicioFisicoID == 0){
                var validacionFechas = inicio <= fin;

                if(validacionFechas == false){
                    return Json(new { success = false, message = "La fecha de inicio no debe ser igual o posterior a la fecha de fin." });
                
                }else{

                var userID = _userManager.GetUserId(User);
                var persona = _context.Personas.Where(p => p.UsuarioID == userID).SingleOrDefault();

                    var newEjercicio = new EjercicioFisico
                    {
                        TipoEjercicioID = tipoEjercicioID,
                        Inicio = inicio,
                        EstadoEmocionalInicio = (EstadoEmocional)estadoEmocionalInicio,
                        EstadoEmocionalFin = (EstadoEmocional)estadoEmocionalFin,
                        Fin = fin,
                        Observaciones = observaciones,
                        LugarID = lugar,
                        PersonaID = persona.PersonaID,
                        EventoDeportivoID = evento
                    };

                    _context.EjerciciosFisicos.Add(newEjercicio);
                    _context.SaveChanges();

                    return Json(true);
                }


            }else{

                var ejercicioFisicoEditar = _context.EjerciciosFisicos.Where(e => e.EjercicioFisicoID == ejercicioFisicoID).SingleOrDefault();

                ejercicioFisicoEditar.Inicio = inicio;
                ejercicioFisicoEditar.Fin = fin;
                ejercicioFisicoEditar.TipoEjercicioID = tipoEjercicioID;
                ejercicioFisicoEditar.EstadoEmocionalFin = estadoEmocionalFin;
                ejercicioFisicoEditar.EstadoEmocionalInicio = estadoEmocionalInicio;
                ejercicioFisicoEditar.Observaciones = observaciones;
                ejercicioFisicoEditar.LugarID = lugar;
                ejercicioFisicoEditar.EventoDeportivoID = evento;

                _context.SaveChanges();

                return Json(true);
            }
        }
    }

    public JsonResult EliminarEjercicioFisico(int ejercicioFisicoID){

        var ejercicioFisicoElimnar = _context.EjerciciosFisicos.Find(ejercicioFisicoID);

        _context.Remove(ejercicioFisicoElimnar);
        _context.SaveChanges();

        return Json(true);
    }


    public JsonResult CrearGraficoEjercicios(int tipoEjercicioId, int mes, int year){

        List<EjerciciosXdia> ejerciciosXdia = new List<EjerciciosXdia>();

        //PARA CREAR EL GRÁFICO DE ACTIVIDADES POR DÍA, PRIMERO DEBO PODER CREAR LOS DÍAS DEL MES A MOSTRAR EN EL GRÁFICO.
        var diasDelMes = DateTime.DaysInMonth(year, mes);
        DateTime fechaMes = new DateTime();
        fechaMes = fechaMes.AddMonths(mes - 1);

        for (int i = 1; i <= diasDelMes; i++)
        {
            var diaDelMesMostrar = new EjerciciosXdia 
            {
                Dia = i,
                Mes = fechaMes.ToString("MMM"),
                CantidadMinutos = 0
            };    
            ejerciciosXdia.Add(diaDelMesMostrar);
        }


        //UNA VEZ CREADOS LOS DIAS DEL MES A MOSTRAR, BUSCAR EN DB LOS EJERCICIOS QUE COINCIDAN CON LOS MÉTODOS DE BÚSQUEDA.
        var ejerciciosBuscar = _context.EjerciciosFisicos.Where(e => e.TipoEjercicioID == tipoEjercicioId && e.Inicio.Month == mes && e.Inicio.Year == year).ToList();

        foreach (var ejercicio in ejerciciosBuscar.OrderBy(e => e.Inicio))
        {
            var ejercicioXdiaMostrar = ejerciciosXdia.Where(e => e.Dia == ejercicio.Inicio.Day).SingleOrDefault();

            if(ejercicioXdiaMostrar != null){
                //Si existe en el listado de ejercicios por dia, sumar la cantidad de minutos obteniendo el intervalo (atributo de vista - ver modelo) de minutos entre fin e inicio-
                ejercicioXdiaMostrar.CantidadMinutos += Convert.ToInt32(ejercicio.IntervaloDeTiempoEjercicio.TotalMinutes);
            }
        }


        return Json(ejerciciosXdia);
    }


    public JsonResult GraficoTipoEjercicios(int mes, int year){

        var tiposEjerciciosView = new List<VistaTipoDeEjercicio>(); 

        var tiposEjercicios = _context.TipoEjercicios.Where(t => t.Eliminado == false).ToList();

        foreach (var ejercicio in tiposEjercicios)
        {
            var ejerciciosMostrar = _context.EjerciciosFisicos.Where(e => e.TipoEjercicioID == ejercicio.TipoEjercicioID && e.Inicio.Month == mes && e.Inicio.Year == year).ToList();

            foreach (var ejercicioActual in ejerciciosMostrar)
            {
                var tipoEjercicioMostrar = tiposEjerciciosView.Where(t => t.TipoEjercicioID == ejercicioActual.TipoEjercicioID).SingleOrDefault();

                if(tipoEjercicioMostrar == null){
                    tipoEjercicioMostrar = new VistaTipoDeEjercicio
                    {
                        TipoEjercicioID = ejercicio.TipoEjercicioID,
                        Descripcion = ejercicio.Nombre,
                        CantidadMinutos = Convert.ToDecimal(ejercicioActual.IntervaloDeTiempoEjercicio.TotalMinutes)
                    };
                    tiposEjerciciosView.Add(tipoEjercicioMostrar);

                }else{
                    tipoEjercicioMostrar.CantidadMinutos += Convert.ToDecimal(ejercicioActual.IntervaloDeTiempoEjercicio.TotalMinutes);
                }
            }
        }

        return Json(tiposEjerciciosView);
    }

}