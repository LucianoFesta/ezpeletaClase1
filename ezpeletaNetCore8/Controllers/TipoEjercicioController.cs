using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ezpeletaNetCore8.Models;
using ezpeletaNetCore8.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ezpeletaNetCore8.Controllers;

[Authorize]
public class TipoEjercicioController : Controller
{
    private ApplicationDbContext _context; //inicializamos el contexto

    //CONTRUCTOR de la clase para traer los datos del contexto (base de datos)
    public TipoEjercicioController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }  

    public JsonResult ListadoTipoEjercicios(int? id)
    {
        var tipoDeEjercicios = _context.TipoEjercicios.Where(t => t.Eliminado == false).ToList();

        if (id.HasValue){
          tipoDeEjercicios = tipoDeEjercicios.Where(t => t.TipoEjercicioID == id).ToList();
        }

        return Json(tipoDeEjercicios);
    }

    public JsonResult GuardarTipoEjercicio(int tipoEjercicioID, string nombre)
    {

        string resultado = "";

        if(!String.IsNullOrEmpty(nombre)){

            //nombre = nombre.ToUpper();

            //Verificamos si se trata de crear un nuevo resgistro o si se edita uno existente.
            if(tipoEjercicioID == 0){

                var existsNombre = _context.TipoEjercicios.Where(t => t.Nombre == nombre).Count();

                if(existsNombre == 0){
                    var tipoEjercicio = new TipoEjercicio { Nombre = nombre };

                    _context.Add(tipoEjercicio);
                    _context.SaveChanges();

                }else if(existsNombre > 0){
                    
                    var tipoExistente = _context.TipoEjercicios.Where(t => t.Nombre == nombre && t.Eliminado == true).SingleOrDefault();
                    tipoExistente.Eliminado = false;

                    _context.SaveChanges();
                    
                }else{
                    resultado = "Ya existe un tipo con dicho nombre.";
                }

            }else{

                var tipoEjercicioEditar = _context.TipoEjercicios.Where(t => t.TipoEjercicioID == tipoEjercicioID).SingleOrDefault();

                if(tipoEjercicioEditar != null){

                    var tipoEjecicioExistente = _context.TipoEjercicios.Where(t => t.Nombre == nombre && t.TipoEjercicioID != tipoEjercicioID).Count();

                    if(tipoEjecicioExistente == 0){
                        tipoEjercicioEditar.Nombre = nombre;
                        _context.SaveChanges();

                    }else{
                        resultado = "Ya existe un registro con la misma descripción";
                    }
                }

            }

        }else{
            resultado = "Debe ingresar un nombre.";

        }

        return Json(resultado);
    }

    public JsonResult EliminarTipoEjercicio(int tipoEjercicioID)
    {
        var tipoEjercicio = _context.TipoEjercicios.Find(tipoEjercicioID);
        var listaEjerciciosFisicos = _context.EjerciciosFisicos.Include(e => e.TipoEjercicio).ToList();

        var existeEjercicio = listaEjerciciosFisicos.Any(e => e.TipoEjercicioID == tipoEjercicioID);

        if (!existeEjercicio)
        {
            tipoEjercicio.Eliminado = true;
            _context.SaveChanges();

            return Json(new { success = true });
        }
        else
        {
            return Json(new { success = false, message = "No puede eliminar el tipo de ejercicio. Existen ejercicios físicos que tienen este tipo de ejercicio." });
        }
    }

}