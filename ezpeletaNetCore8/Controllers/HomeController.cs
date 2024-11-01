using System.Diagnostics; //Contexto principal
using Microsoft.AspNetCore.Mvc;
using ezpeletaNetCore8.Models;
using Microsoft.AspNetCore.Identity;
using SQLitePCL;
using ezpeletaNetCore8.Data;

namespace ezpeletaNetCore8.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;

    private readonly RoleManager<IdentityRole> _roleManager;

    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
    {
        _logger = logger;

        _context = context;

        _userManager = userManager;

        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        await CrearRoles();
        return View();
    }

    public async Task<JsonResult> CrearRoles()
    {
        var rolUsuarioExiste = _context.Roles.Where(r => r.Name == "Deportista").SingleOrDefault();
        var rolAdminExiste = _context.Roles.Where(r => r.Name == "Administrador").SingleOrDefault();

        if(rolUsuarioExiste == null)
        {
            var rol = await _roleManager.CreateAsync(new IdentityRole("Deportista"));
        }

        if(rolAdminExiste == null)
        {
            var rol = await _roleManager.CreateAsync(new IdentityRole("Administrador"));
        }

        bool userAdminCreado = false;
        var userAdmin = _context.Users.Where(u => u.Email == "admin@vidaactiva.com").SingleOrDefault();

        if(userAdmin == null){
            var admin = new IdentityUser { UserName = "admin@vidaactiva.com", Email = "admin@vidaactiva.com" };
            var result = await _userManager.CreateAsync(admin, "vidaactiva2024");

            await _userManager.AddToRoleAsync(admin, "Administrador");
            userAdminCreado = result.Succeeded; 
        }

        return Json(userAdminCreado);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
