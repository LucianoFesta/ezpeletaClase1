using System.Diagnostics; //Contexto principal
using Microsoft.AspNetCore.Mvc;
using ezpeletaNetCore8.Models;
using ezpeletaNetCore8.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ezpeletaNetCore8.Controllers;

public class LugarController : Controller
{
    private readonly ILogger<LugarController> _logger;
        private ApplicationDbContext _context; //inicializamos el contexto

        private UserManager<IdentityUser> _userManager;

    public LugarController(ILogger<LugarController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _logger = logger;

        _context = context;

        _userManager = userManager;
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

    public JsonResult SaveLugar(int lugarID, string lugar){
        var userID = _userManager.GetUserId(User);
        var persona = _context.Personas.Where(p => p.UsuarioID == userID).SingleOrDefault();

        var lugarExiste = _context.Lugares.Where(l => l.Nombre.ToLower() == lugar.ToLower()).Count();

        if(lugarExiste > 0)
        {
            return Json(new { success = false, message = "El lugar ya existe en la base de datos." });
        }

        if(string.IsNullOrEmpty(lugar))
        {
            return Json(new { success = false, message = "Por favor, completa todos los campos para poder crear un lugar." });

        }else{

            if(lugarID == 0){

                var newLugar = new Lugar
                {
                    LugarID = lugarID,
                    Nombre = lugar,
                    PersonaID = persona.PersonaID
                };

                _context.Lugares.Add(newLugar);
                _context.SaveChanges();

                return Json(true);
            }else{

                var lugarEditar = _context.Lugares.Where(l => l.LugarID == lugarID).SingleOrDefault();

                lugarEditar.LugarID = lugarID;
                lugarEditar.Nombre = lugar;

                _context.SaveChanges();

                return Json(true);
            }
        }
    }

    public JsonResult ListadoLugares(int? idLugar)
    {
        var userID = _userManager.GetUserId(User);
        var persona = _context.Personas.Where(p => p.UsuarioID == userID).SingleOrDefault();
        
        var listadoLugares = new List<Lugar>();

        if(User.IsInRole("Deportista"))
        {
            listadoLugares = _context.Lugares
                .Where(l => l.PersonaID == persona.PersonaID)
                .ToList();
        }else{
            listadoLugares = _context.Lugares.ToList();
        }

        if(listadoLugares.Count > 0)
        {
            var lugares = listadoLugares.Select(e => new Lugar(){
                LugarID = e.LugarID,
                Nombre = e.Nombre,
                PersonaID = e.PersonaID
            }).ToList();

            return Json(new { success = true, lista = lugares });
        }

        return Json(false);
    }

    public JsonResult EliminarLugar(int id)
    {
        var lugarEliminar = _context.Lugares.Where(l => l.LugarID == id).SingleOrDefault();
        var ejerciciosEnLugar = _context.EjerciciosFisicos.Include(t => t.Lugar).ToList();

        var existEjercicio = ejerciciosEnLugar.Any(e => e.LugarID == id);

        if(!existEjercicio)
        {
            _context.Remove(lugarEliminar);
            _context.SaveChanges();

            return Json(true);
        }

        return Json(new { success = false, message = "No se puede eliminar el lugar ya que existen ejercicios guardados con dicho lugar." });

    }
}
