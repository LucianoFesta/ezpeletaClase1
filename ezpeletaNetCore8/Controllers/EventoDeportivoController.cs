using System.Diagnostics; //Contexto principal
using Microsoft.AspNetCore.Mvc;
using ezpeletaNetCore8.Models;
using ezpeletaNetCore8.Data;
using Microsoft.EntityFrameworkCore;

namespace ezpeletaNetCore8.Controllers;

public class EventoDeportivoController : Controller
{
    private readonly ILogger<EventoDeportivoController> _logger;
        private ApplicationDbContext _context; //inicializamos el contexto

    public EventoDeportivoController(ILogger<EventoDeportivoController> logger, ApplicationDbContext context)
    {
        _logger = logger;

        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

public JsonResult SaveEvento(int eventoID, string evento)
{

    if(eventoID == 0){
        // Busca el evento en la base de datos sin importar si está eliminado o no
        var eventoExistente = _context.EventosDeportivos
                                    .Where(e => e.Descripcion.ToLower() == evento.ToLower())
                                    .SingleOrDefault();

        if (eventoExistente == null)
        {
            var newEvento = new EventoDeportivo
            {
                Descripcion = evento,
                Eliminado = false
            };

            _context.EventosDeportivos.Add(newEvento);
            _context.SaveChanges();

            return Json(new { success = true, message = "El evento ha sido creado exitosamente." });
        }

        if (eventoExistente.Eliminado)
        {
            eventoExistente.Eliminado = false;
            _context.SaveChanges();

            return Json(new { success = true, message = "El evento ha sido restaurado." });
        }

        return Json(new { success = false, message = "El evento ya existe y no está eliminado." });

    }else{
        var eventoEditar = _context.EventosDeportivos.Where(e => e.EventoID == eventoID).SingleOrDefault();

        if(eventoEditar != null)
        {
            eventoEditar.Descripcion = evento;
            _context.SaveChanges();

            return Json(new { success = true, message = "El evento ha sido editado." });
        }
        
        return Json(new { success = true, message = "El evento a editar no ha sido encontrado." });

    }
}


    public JsonResult ListadoEventos(int? idEvento)
    {
        var listadoEventos = _context.EventosDeportivos.Where(e => e.Eliminado == false).ToList();

        if(idEvento > 0)
        {
            listadoEventos = listadoEventos.Where(l => l.EventoID == idEvento).ToList();
        }

        if(listadoEventos.Count > 0)
        {
            return Json(new { success = true, lista = listadoEventos });
        }

        return Json(false);
    }

    public JsonResult EliminarEvento(int id)
    {
        var eventoEliminar = _context.EventosDeportivos.Where(e => e.EventoID == id).SingleOrDefault();
        var ejerciciosEnEventos = _context.EjerciciosFisicos.Include(t => t.EventoDeportivo).ToList();

        var existEjercicio = ejerciciosEnEventos.Any(e => e.EventoDeportivoID == id);

        if(!existEjercicio)
        {
            eventoEliminar.Eliminado = true;
            _context.SaveChanges();

            return Json(true);
        }

        return Json(new { success = false, message = "No se puede eliminar el evento ya que existen ejercicios guardados con dicho evento." });

    }
}
