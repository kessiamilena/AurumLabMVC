using Microsoft.AspNetCore.Mvc;

namespace AurumLab.Controllers
{
    public class HomeController : Controller
    {
       public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerificarAcesso()
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return RedirectToAction("Dashboard", "Dashboard");
        }
    }
    
}
