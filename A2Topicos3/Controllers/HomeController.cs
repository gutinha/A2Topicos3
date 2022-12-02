using A2Topicos3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace A2Topicos3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        TpContext db = new TpContext();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.username = HttpContext.Session.GetString("nome_user");
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Login")]
        public IActionResult Login(string email, string senha)
        {
            var user = db.Usuarios.Where(u => u.Email == email && u.Senha == Util.hash(email+senha)).FirstOrDefault();
            if (user != null)
            {
                HttpContext.Session.SetInt32("id_user", user.Id);
                HttpContext.Session.SetString("nome_user", user.Nome);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.msg = "Usuário ou senha inválidos";
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("id_user");
            HttpContext.Session.Remove("nome_user");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}