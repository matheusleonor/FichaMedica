using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciamentoDeFichasMedicas.Models;

namespace GerenciamentoDeFichasMedicas.Controllers
{
    public class FuncoesController : Controller
    {
        private readonly HospitalContext _context;

        public FuncoesController(HospitalContext context)
        {
            _context = context;
        }

        public IActionResult GetFormData()
        {
            var funcoes = _context.Funcoes.ToList();
            return Json(funcoes);
        }

        // GET: Funcoes
        public async Task<IActionResult> Index()
        {
              return _context.Funcoes != null ? 
                          View(await _context.Funcoes.ToListAsync()) :
                          Problem("Entity set 'HospitalContext.Funcoes'  is null.");
        }

        // GET: Funcoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Funcoes == null)
            {
                return NotFound();
            }

            var funcoes = await _context.Funcoes
                .FirstOrDefaultAsync(m => m.FuncaoId == id);
            if (funcoes == null)
            {
                return NotFound();
            }

            return View(funcoes);
        }

        // GET: Funcoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Funcoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FuncaoId,NomeFuncao")] Funcoes funcoes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcoes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(funcoes);
        }

        // GET: Funcoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Funcoes == null)
            {
                return NotFound();
            }

            var funcoes = await _context.Funcoes.FindAsync(id);
            if (funcoes == null)
            {
                return NotFound();
            }
            return View(funcoes);
        }

        // POST: Funcoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FuncaoId,NomeFuncao")] Funcoes funcoes)
        {
            if (id != funcoes.FuncaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcoes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncoesExists(funcoes.FuncaoId))
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
            return View(funcoes);
        }

        // GET: Funcoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Funcoes == null)
            {
                return NotFound();
            }

            var funcoes = await _context.Funcoes
                .FirstOrDefaultAsync(m => m.FuncaoId == id);
            if (funcoes == null)
            {
                return NotFound();
            }

            return View(funcoes);
        }

        // POST: Funcoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Funcoes == null)
            {
                return Problem("Entity set 'HospitalContext.Funcoes'  is null.");
            }
            var funcoes = await _context.Funcoes.FindAsync(id);
            if (funcoes != null)
            {
                _context.Funcoes.Remove(funcoes);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FuncoesExists(int id)
        {
          return (_context.Funcoes?.Any(e => e.FuncaoId == id)).GetValueOrDefault();
        }
    }
}
