using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using A2Topicos3.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Newtonsoft.Json;

namespace A2Topicos3.Controllers
{
    public class RevisaoController : Controller
    {
        private TpContext _context = new TpContext();
        public INotyfService _notifyService { get; }
        public RevisaoController(INotyfService notifyService)
        {
            _notifyService = notifyService;
        }

        // GET: Revisao Admin
        public async Task<IActionResult> Index()
        {
            var tpContext = _context.Revisaos.Include(r => r.Carro).Include(r => r.Usuario);
            return View(await tpContext.ToListAsync());
        }
        // GET: Revisao user
        public async Task<IActionResult> IndexUser()
        {
            var tpContext = _context.Revisaos.Include(r => r.Carro).Include(r => r.Usuario);
            return View(await tpContext.ToListAsync());
        }

        // GET: Revisao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Revisaos == null)
            {
                return NotFound();
            }

            var revisao = await _context.Revisaos
                .Include(r => r.Carro)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (revisao == null)
            {
                return NotFound();
            }

            return View(revisao);
        }

        public async Task<IActionResult> DetailsUser(int? id)
        {
            if (id == null || _context.Revisaos == null)
            {
                return NotFound();
            }

            var revisao = await _context.Revisaos
                .Include(r => r.Carro)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (revisao == null)
            {
                return NotFound();
            }

            return View(revisao);
        }

        // GET: Revisao/Create
        public IActionResult Create()
        {
            ViewData["CarroId"] = new SelectList(_context.Carros, "Id", "Nome");
            return View();
        }

        // POST: Revisao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descricao,DataRevisao,CarroId")] Revisao revisao)
        {
            try
            {
                var a = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
                revisao.UsuarioId = a.Id;
                _context.Add(revisao);
                _context.Logs.Add(new Log
                {
                    LogDateTime = DateTime.Now,
                    Texto = "Usuário " + a.Nome + " agendou uma revisão"
                });
                await _context.SaveChangesAsync();
                _notifyService.Success("Revisão agendada com sucesso!");
                return RedirectToAction("IndexUser", "Revisao");
            }
            catch (Exception e)
            {
                _notifyService.Error("Erro ao cadastrar revisão! '" + e.Message + "'");
                ViewData["CarroId"] = new SelectList(_context.Carros, "Id", "Nome", revisao.CarroId);
                return RedirectToAction("Index", "Revisao");
            }
        }

        // GET: Revisao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Revisaos == null)
            {
                return NotFound();
            }

            var revisao = await _context.Revisaos.FindAsync(id);
            if (revisao == null)
            {
                return NotFound();
            }
            ViewData["CarroId"] = new SelectList(_context.Carros, "Id", "Nome", revisao.CarroId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", revisao.UsuarioId);
            return View(revisao);
        }

        // POST: Revisao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,DataRevisao,CarroId,UsuarioId,Status")] Revisao revisao)
        {
            if (id != revisao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var a = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
                    _context.Update(revisao);
                    _context.Logs.Add(new Log
                    {
                        LogDateTime = DateTime.Now,
                        Texto = "Usuário " + a.Nome + " com id:" + a.Id + " alterou uma revisão"
                    });
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Revisão atualizada com sucesso!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RevisaoExists(revisao.Id))
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
            ViewData["CarroId"] = new SelectList(_context.Carros, "Id", "Nome", revisao.CarroId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", revisao.UsuarioId);
            return View(revisao);
        }

        // GET: Revisao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Revisaos == null)
            {
                return NotFound();
            }

            var revisao = await _context.Revisaos
                .Include(r => r.Carro)
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (revisao == null)
            {
                return NotFound();
            }

            return View(revisao);
        }

        // POST: Revisao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Revisaos == null)
            {
                return Problem("Entity set 'TpContext.Revisaos'  is null.");
            }
            var revisao = await _context.Revisaos.FindAsync(id);
            if (revisao != null)
            {
                var a = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
                _context.Logs.Add(new Log
                {
                    LogDateTime = DateTime.Now,
                    Texto = "Usuário " + a.Nome + " com id: " + a.Id + " deletou uma revisão"
                });
                _context.Revisaos.Remove(revisao);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RevisaoExists(int id)
        {
          return _context.Revisaos.Any(e => e.Id == id);
        }
    }
}
