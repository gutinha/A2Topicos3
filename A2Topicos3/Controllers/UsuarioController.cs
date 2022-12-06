using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using A2Topicos3.Models;
using Newtonsoft.Json;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace A2Topicos3.Controllers
{
    public class UsuarioController : Controller
    {
        private TpContext _context = new TpContext();
        public INotyfService _notifyService { get; }
        public UsuarioController(INotyfService notifyService)
        {
            _notifyService = notifyService;
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Email,Senha,DataNascimento,Rg,Cpf,Cnpj")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var valid = _context.Usuarios.Where(x => x.Email == usuario.Email).FirstOrDefault();
                if (valid.Id != 0 || valid.Id != null)
                {
                        _notifyService.Warning("Email já cadastrado");
                        return RedirectToAction("Login", "Home");
                }
                else
                {
                    usuario.Ativo = true;
                    usuario.Senha = Util.hash(usuario.Email + usuario.Senha);
                    usuario.Permissos = new List<Permisso>
                    {
                    new Permisso() { IdUsuario = usuario.Id, Permissao = "User" }
                    };
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    var user = _context.Usuarios.Where(x => x.Email == usuario.Email).FirstOrDefault();
                    HttpContext.Session.SetString("user", JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }));
                    return RedirectToAction("Index", "Home");
                }
            }
            _notifyService.Error("Erro ao criar usuário");
            return RedirectToAction("Index", "Home"); ;
        }

        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin([Bind("Nome,Email,Senha,DataNascimento,Rg,Cpf,Cnpj")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var valid = _context.Usuarios.Where(x => x.Email == usuario.Email).FirstOrDefault();
                if (valid.Id != 0 || valid.Id != null)
                {
                    if(valid.Ativo == false)
                    {
                        valid.Ativo = true;
                    }
                    valid.Permissos = new List<Permisso>
                    {
                    new Permisso() { IdUsuario = usuario.Id, Permissao = "Admin" }
                    };
                    _context.Add(valid).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Usuário criado com sucesso");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    usuario.Ativo = true;
                    usuario.Senha = Util.hash(usuario.Email + usuario.Senha);
                    usuario.Permissos = new List<Permisso>
                    {
                    new Permisso() { IdUsuario = usuario.Id, Permissao = "Admin" }
                    };
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Cadastrado com sucesso!");
                    return RedirectToAction("Index", "Home");
                }
            }
            _notifyService.Error("Erro ao criar usuário");
            return RedirectToAction("Index", "Home");
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Senha,DataNascimento,Rg,Cpf,Cnpj,Ativo")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'TpContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                var str = HttpContext.Session.GetString("user");
                var usu = JsonConvert.DeserializeObject<Usuario>(str);
                if (usu.Id == usuario.Id)
                {
                    _notifyService.Error("Não é possível excluir o usuário logado");
                    return RedirectToAction("Index", "Usuario");
                }
                else
                {
                    usuario.Ativo = false;
                }
            }
            
            await _context.SaveChangesAsync();
            _notifyService.Success("Usuário deletado com sucesso");
            return RedirectToAction("Index", "Usuario");
        }

        private bool UsuarioExists(int id)
        {
          return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
