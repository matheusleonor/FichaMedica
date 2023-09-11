using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciamentoDeFichasMedicas.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;
using System.Net;

namespace GerenciamentoDeFichasMedicas.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HospitalContext _context;
        private readonly IPasswordHasher<Usuarios> _passwordHasher;

        public UsuariosController(HospitalContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var hospitalContext = _context.Usuarios.Include(u => u.Funcao);
            return View(await hospitalContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .Include(u => u.Funcao)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["FuncaoId"] = new SelectList(_context.Funcoes, "FuncaoId", "FuncaoId");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeUsuario,SenhaHash,FuncaoId")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                // Criptografar a senha usando SHA256
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(usuarios.SenhaHash);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    usuarios.SenhaHash = hashedPassword;
                }

                _context.Add(usuarios);
                await _context.SaveChangesAsync();

                TempData["MensagemSucesso"] = "Conta cadastrada com sucesso!";
                return RedirectToAction("Index", "Home");
            }

            ViewData["FuncaoId"] = new SelectList(_context.Funcoes, "FuncaoId", "NomeFuncao", usuarios.FuncaoId);
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }
            ViewData["FuncaoId"] = new SelectList(_context.Funcoes, "FuncaoId", "FuncaoId", usuarios.FuncaoId);
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,NomeUsuario,SenhaHash,FuncaoId")] Usuarios usuarios)
        {
            if (id != usuarios.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuarios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuariosExists(usuarios.UsuarioId))
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
            ViewData["FuncaoId"] = new SelectList(_context.Funcoes, "FuncaoId", "FuncaoId", usuarios.FuncaoId);
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios
                .Include(u => u.Funcao)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'HospitalContext.Usuarios'  is null.");
            }
            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios != null)
            {
                _context.Usuarios.Remove(usuarios);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuariosExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.UsuarioId == id)).GetValueOrDefault();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Usuarios.SingleOrDefaultAsync(u => u.NomeUsuario == usuario.NomeUsuario);

                if (user != null)
                {
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(usuario.SenhaHash);
                        byte[] hashBytes = sha256.ComputeHash(bytes);
                        string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                        if (hashedPassword == user.SenhaHash)
                        {

                            if (user.FuncaoId == 1) // Paciente
                            {
                                return RedirectToAction("Index", "FichasMedicas", new { pacienteId = user.UsuarioId, funcaoId = user.FuncaoId });
                            }
                            else if (user.FuncaoId == 2) // Médico
                            {
                                return RedirectToAction("Index", "FichasMedicas", new { medicoId = user.UsuarioId, funcaoId = user.FuncaoId });
                            }

                        }
                    }
                }

                TempData["MensagemErro"] = "Nome de usuário ou senha inválidos.";
                TempData["ExibirModalErro"] = true;
                return RedirectToAction("Index", "Home");
            }

            TempData["MensagemErro"] = "Nome de usuário ou senha inválidos.";
            TempData["ExibirModalErro"] = true;
            return RedirectToAction("Index", "Home");
        }


    }
}
