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
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));
                db.Logs.Add(new Log
                {
                    LogDateTime = DateTime.Now,
                    Texto = "Login do usuário " + user.Nome + " com id:" + user.Id
                }) ;
                db.SaveChanges();
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
            var str = HttpContext.Session.GetString("user");
            var usu = JsonConvert.DeserializeObject<Usuario>(str);
            HttpContext.Session.Remove("user");
            db.Logs.Add(new Log
            {
                LogDateTime = DateTime.Now,
                Texto = "Logout do usuário " + usu.Nome + " com id:" + usu.Id
            });
            db.SaveChanges();
            _notifyService.Success("Logout realizado");
            return RedirectToAction("Login", "Home");
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