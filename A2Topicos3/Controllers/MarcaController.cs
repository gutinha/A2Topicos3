using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using A2Topicos3.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Newtonsoft.Json;

namespace A2Topicos3.Controllers
{
    public class MarcaController : Controller
    {
        private TpContext _context = new TpContext();
        public INotyfService _notifyService { get; }
        public MarcaController(INotyfService notifyService)
        {
            _notifyService = notifyService;
        }

        // GET: Marca
        public async Task<IActionResult> Index()
        {
              return View(await _context.Marcas.ToListAsync());
        }

        // GET: Marca/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // GET: Marca/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Marca/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,Descricao,Imagem")] Marca marca)
        {
            if (ModelState.IsValid)
            {
                var a = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
                _context.Add(marca);
                _context.Logs.Add(new Log { LogDateTime = DateTime.Now, Texto = "Usuário " + a.Nome + " com id: " + a.Id + " criou a marca: " + marca.Nome });
                await _context.SaveChangesAsync();
                _notifyService.Success("Marca cadastrada com sucesso!");
                return RedirectToAction("Index","Home");
            }
            _notifyService.Error("Erro ao cadastrar marca!");
            return RedirectToAction("Index", "Home");
        }

        // GET: Marca/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }

        // POST: Marca/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao")] Marca marca)
        {
            if (id != marca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var a = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
                    _context.Update(marca);
                    _context.Logs.Add(new Log { LogDateTime = DateTime.Now, Texto = "Usuário " + a.Nome + " com id: " +a.Id+" editou a marca: " + marca.Nome });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcaExists(marca.Id))
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
            return View(marca);
        }

        // GET: Marca/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Marcas == null)
            {
                return NotFound();
            }

            var marca = await _context.Marcas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (marca == null)
            {
                return NotFound();
            }

            return View(marca);
        }

        // POST: Marca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Marcas == null)
            {
                return Problem("Entity set 'TpContext.Marcas'  is null.");
            }
            var marca = await _context.Marcas.FindAsync(id);
            if (marca != null)
            {
                var a = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
                _context.Logs.Add(new Log
                {
                    LogDateTime = DateTime.Now,
                    Texto = "Usuário "   + a.Nome + " com id: " + a.Id + " deletou a marca: " + marca.Nome });
                _context.Marcas.Remove(marca);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarcaExists(int id)
        {
          return _context.Marcas.Any(e => e.Id == id);
        }
    }
}
