using Microsoft.AspNetCore.Mvc;
using ezpeletaNetCore8.Models;
using ezpeletaNetCore8.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ezpeletaNetCore8.Controllers;

[Authorize]
public class EjercicioFisicoController : Controller
{
    private ApplicationDbContext _context; //inicializamos el contexto

    //CONTRUCTOR de la clase para traer los datos del contexto (base de datos)
    public EjercicioFisicoController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {

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

        tipoEjercicios.Add(new TipoEjercicio
        {
            TipoEjercicioID = 0,
            Nombre = "[Seleccione...]"
        });

        ViewBag.TipoEjercicioID = new SelectList(tipoEjercicios.OrderBy(c => c.Nombre), "TipoEjercicioID", "Nombre");

        return View();
    }

    
    public JsonResult ListadoEjerciciosFisicos(int? id)
    {
        //Se carga el tipo de ejercicio relacionado
        var ejerciciosFisicos = _context.EjerciciosFisicos.Include(e => e.TipoEjercicio).ToList();

        if (id.HasValue){   
            ejerciciosFisicos = ejerciciosFisicos.Where(e => e.EjercicioFisicoID == id).ToList();
        
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
            NombreTipoEjercicio = e.TipoEjercicio.Nombre
            
        }).ToList();

        return Json(newListEjerciciosFisicos);
    }

    public JsonResult SaveEjercicio(int ejercicioFisicoID, int tipoEjercicioID, DateTime inicio, EstadoEmocional estadoEmocionalInicio, EstadoEmocional estadoEmocionalFin, DateTime fin, string observaciones){

        if(tipoEjercicioID <= 0 || inicio == DateTime.MinValue || estadoEmocionalInicio == 0 || estadoEmocionalFin == 0 || fin == DateTime.MinValue || string.IsNullOrEmpty(observaciones))
        {
            return Json(new { success = false, message = "Por favor, completa todos los campos para poder crear un ejercicio físico." });

        }else{

            if(ejercicioFisicoID == 0){
                var validacionFechas = inicio <= fin;

                if(validacionFechas == false){
                    return Json(new { success = false, message = "La fecha de inicio no debe ser igual o posterior a la fecha de fin." });
                
                }else{

                    var newEjercicio = new EjercicioFisico
                    {
                        TipoEjercicioID = tipoEjercicioID,
                        Inicio = inicio,
                        EstadoEmocionalInicio = (EstadoEmocional)estadoEmocionalInicio,
                        EstadoEmocionalFin = (EstadoEmocional)estadoEmocionalFin,
                        Fin = fin,
                        Observaciones = observaciones
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

}