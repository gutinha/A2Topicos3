using A2Topicos3.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace A2Topicos3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public INotyfService _notifyService { get; }
        TpContext db = new TpContext();

        public HomeController(ILogger<HomeController> logger, INotyfService notifyService)
        {
            _notifyService = notifyService;
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
            var user = db.Usuarios.Where(u => u.Email == email && u.Senha == Util.hash(email + senha) && u.Ativo == true).FirstOrDefault();
            if (user != null)
            {
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user));
                _notifyService.Success("Login realizado com sucesso!");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _notifyService.Error("Usuário ou senha inválidos");
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("user");
            HttpContext.Session.Remove("perm");
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