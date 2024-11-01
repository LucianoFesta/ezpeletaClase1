using ezpeletaNetCore8.Data;
using ezpeletaNetCore8.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ezpeletaNetCore8.Controllers;

public class RegisterController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _rolManager;

    public RegisterController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolManager )
    {
        _context = context;
        _userManager = userManager;
        _rolManager = rolManager;
    }

    public IActionResult Index()
    {

        //Crear lista de selectListItem
        var selectListItem = new List<SelectListItem>
        {
            new SelectListItem{ Value = "0", Text = "[Seleccione..]"}
        };

        //Obtener las opciones del enum
        var enumValues = Enum.GetValues(typeof(Genero)).Cast<Genero>();
        
        //Convertir las opciones del enum en SelectItem
        selectListItem.AddRange(enumValues.Select(e => new SelectListItem
        {
            Value = e.GetHashCode().ToString(),
            Text = e.ToString().ToUpper()
        }));

        //Pasar la lista a la vista
        ViewBag.Genero = selectListItem.OrderBy(t => t.Text).ToList();

        return View();
    }

public async Task<JsonResult> GuardarPersona(
    string nombreCompleto, decimal peso, decimal altura, DateTime fechaNacimiento,
    string email, Genero genero, string password, string confirmPassword)
{
    await GuardarUsuario(email, password);

    var userRegistrado = _context.Users.SingleOrDefault(u => u.Email == email);

    if (userRegistrado != null)
    {
        var persona = new Persona
        {
            UsuarioID = userRegistrado.Id,
            NombreCompleto = nombreCompleto,
            FechaNacimiento = fechaNacimiento,
            Genero = genero,
            Peso = peso,
            Altura = altura
        };

        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();  // Asegura que los cambios se guarden

        return Json(new { result = true });
    }
    else
    {
        return Json(new { result = false, message = "Ocurrió un error al guardar la persona." });
    }
}
    
   public async Task<JsonResult> GuardarUsuario( string email, string password )
    {
        //CREAR LA VARIABLE USUARIO CON TODOS LOS DATOS
        var user = new IdentityUser { UserName = email, Email = email };

        //EJECUTAR EL METODO CREAR USUARIO PASANDO COMO PARAMETRO EL OBJETO CREADO ANTERIORMENTE Y LA CONTRASEÑA DE INGRESO
        var result = await _userManager.CreateAsync(user, password);

        //BUSCAR POR MEDIO DE CORREO ELECTRONICO ESE USUARIO CREADO PARA BUSCAR EL ID
        var usuario = _context.Users.Where(u => u.Email == email).SingleOrDefault();

        if(usuario != null){
            await _userManager.AddToRoleAsync(usuario, "Deportista");
            
            return Json(new { result = true, user = usuario });

        }else{
            return Json(new { result = false, message = "Ocurrió un error al guardar el usuario." });
        }
    }

}