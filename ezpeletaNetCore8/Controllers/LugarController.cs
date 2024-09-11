using System.Diagnostics; //Contexto principal
using Microsoft.AspNetCore.Mvc;
using ezpeletaNetCore8.Models;
using ezpeletaNetCore8.Data;

namespace ezpeletaNetCore8.Controllers;

public class LugarController : Controller
{
    private readonly ILogger<LugarController> _logger;
        private ApplicationDbContext _context; //inicializamos el contexto

    public LugarController(ILogger<LugarController> logger, ApplicationDbContext context)
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

    public JsonResult SaveLugar(int lugarID, string lugar){

        if(string.IsNullOrEmpty(lugar))
        {
            return Json(new { success = false, message = "Por favor, completa todos los campos para poder crear un lugar." });

        }else{

            if(lugarID == 0){

                var newLugar = new Lugar
                {
                    LugarID = lugarID,
                    Nombre = lugar
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
}
