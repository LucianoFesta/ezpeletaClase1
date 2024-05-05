using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ezpeletaNetCore8.Models;
using ezpeletaNetCore8.Data;

namespace ezpeletaNetCore8.Controllers;

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
        var tipoDeEjercicios = _context.TipoEjercicios.ToList();

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

                }else{
                    resultado = "Ya existe un registro con la misma descripción";
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

        _context.Remove(tipoEjercicio);
        _context.SaveChanges();

        return Json(true);

    }
}