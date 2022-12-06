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
    public class EnderecoController : Controller
    {
        private TpContext _context = new TpContext();
        public INotyfService _notifyService { get; }
        public EnderecoController(INotyfService notifyService)
        {
            _notifyService = notifyService;
        }

        // GET: Endereco
        public async Task<IActionResult> Index()
        {
            var tpContext = _context.Enderecos.Include(e => e.Usuario);
            return View(await tpContext.ToListAsync());
        }

        // GET: Endereco/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Enderecos == null)
            {
                return NotFound();
            }

            var endereco = await _context.Enderecos
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endereco == null)
            {
                return NotFound();
            }

            return View(endereco);
        }

        // GET: Endereco/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome");
            return View();
        }

        // POST: Endereco/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Endereco1,Numero,Complemento,Bairro,Cidade,Estado,Cep")] Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                var a = JsonConvert.DeserializeObject<Usuario>(HttpContext.Session.GetString("user"));
                endereco.UsuarioId = (a.Id);
                _context.Add(endereco);
                await _context.SaveChangesAsync();
                _notifyService.Success("Endereço cadastrado com sucesso!");
                return RedirectToAction(nameof(Index));
            }
            _notifyService.Error("Erro ao cadastrar endereço!");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", endereco.UsuarioId);
            return View(endereco);
        }

        // GET: Endereco/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Enderecos == null)
            {
                return NotFound();
            }

            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", endereco.UsuarioId);
            return View(endereco);
        }

        // POST: Endereco/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Endereco1,Numero,Complemento,Bairro,Cidade,Estado,Cep,UsuarioId")] Endereco endereco)
        {
            if (id != endereco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(endereco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnderecoExists(endereco.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notifyService.Success("Endereço atualizado com sucesso.");
                return RedirectToAction(nameof(Index));
            }
            _notifyService.Error("Algo deu errado.");
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nome", endereco.UsuarioId);
            return View(endereco);
        }

        // GET: Endereco/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Enderecos == null)
            {
                return NotFound();
            }

            var endereco = await _context.Enderecos
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (endereco == null)
            {
                return NotFound();
            }

            return View(endereco);
        }

        // POST: Endereco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Enderecos == null)
            {
                return Problem("Entity set 'TpContext.Enderecos'  is null.");
            }
            var endereco = await _context.Enderecos.FindAsync(id);
            if (endereco != null)
            {
                _context.Enderecos.Remove(endereco);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnderecoExists(int id)
        {
          return _context.Enderecos.Any(e => e.Id == id);
        }
    }
}
