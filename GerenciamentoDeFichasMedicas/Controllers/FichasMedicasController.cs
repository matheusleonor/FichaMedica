using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GerenciamentoDeFichasMedicas.Models;
using Microsoft.AspNetCore.Hosting;

namespace GerenciamentoDeFichasMedicas.Controllers
{
    public class FichasMedicasController : Controller
    {
        private readonly HospitalContext _context;

        private readonly IWebHostEnvironment hostingEnvironment;

        public FichasMedicasController(HospitalContext context, IWebHostEnvironment environment)
        {
            _context = context;
            hostingEnvironment = environment;

        }

        // GET: FichasMedicas
        public async Task<IActionResult> Index(int? pacienteId, int? medicoId, int? funcaoId)
        {
            if (!medicoId.HasValue && !pacienteId.HasValue)
            {
                // Redirecionar para a página inicial (Home) se os parâmetros não estiverem presentes
                return RedirectToAction("Index", "Home");
            }

            IQueryable<FichasMedicas> fichasMedicasQuery = _context.FichasMedicas;

            if (pacienteId.HasValue)
            {
                fichasMedicasQuery = fichasMedicasQuery.Where(f => f.PacienteId == pacienteId);
            }
            else if (medicoId.HasValue)
            {
                fichasMedicasQuery = fichasMedicasQuery.Where(f => f.MedicoId == medicoId);
            }

            var fichasMedicas = await fichasMedicasQuery
                .Include(f => f.Medico)
                .Include(f => f.Paciente)
                .ToListAsync();

            ViewBag.FuncaoId = funcaoId;
            ViewBag.MedicoId = medicoId;
            ViewBag.PacienteId = pacienteId;

            return View(fichasMedicas);
        }


        // GET: FichasMedicas/Details/5
        public async Task<IActionResult> Details(int? id, int? pacienteId, int? medicoId, int? funcaoId)
        {
            if (id == null || _context.FichasMedicas == null)
            {
                return NotFound();
            }

            var fichasMedicas = await _context.FichasMedicas
                .Include(f => f.Medico)
                .Include(f => f.Paciente)
                .FirstOrDefaultAsync(f => f.FichaId == id &&
                                           (f.PacienteId == pacienteId || f.MedicoId == medicoId));

            ViewBag.FuncaoId = funcaoId;
            ViewBag.MedicoId = medicoId;
            ViewBag.PacienteId = pacienteId;

            if (fichasMedicas == null)
            {
                return NotFound();
            }

            return View(fichasMedicas);
        }



        // GET: FichasMedicas/Create
        public IActionResult Create(int medicoId = 0)
        {

            ViewData["MedicoId"] = new SelectList(_context.Usuarios.Where(u => u.FuncaoId == 2), "UsuarioId", "NomeUsuario");
            ViewData["PacienteId"] = new SelectList(_context.Usuarios.Where(u => u.FuncaoId == 1), "UsuarioId", "NomeUsuario");

            if (medicoId != 0)
            {
                ViewBag.usuarioId = medicoId;
                ViewBag.funcaoId = 2;
            }

            return View();
        }

        // POST: FichasMedicas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FichaId,PacienteId,MedicoId,NomeCompleto,Foto,Cpf,Celular,Endereco,TextoRico,UserId")] FichasMedicas fichasMedicas, int usuarioId, IFormFile Foto)
        {

            if (ModelState.IsValid)
            {

                if (Foto != null && Foto.Length > 0)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Foto.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    Foto.CopyTo(new FileStream(filePath, FileMode.Create));

                    fichasMedicas.Foto = uniqueFileName; // Salve o nome do arquivo na propriedade Foto da sua classe
                }

                _context.Add(fichasMedicas);
                await _context.SaveChangesAsync();


            }

            ViewData["MedicoId"] = new SelectList(_context.Usuarios, "UsuarioId", "NomeUsuario", fichasMedicas.MedicoId);
            ViewData["PacienteId"] = new SelectList(_context.Usuarios, "UsuarioId", "NomeUsuario", fichasMedicas.PacienteId);

            // Redirecionar para a página correta com base no ID do usuário
            return RedirectToAction("Index", "FichasMedicas", new { medicoId = usuarioId, funcaoId = 2 });
        }



        // GET: FichasMedicas/Edit/5
        public async Task<IActionResult> Edit(int? id, int medicoId)
        {
            if (id == null || _context.FichasMedicas == null)
            {
                return NotFound();
            }

            var fichasMedicas = await _context.FichasMedicas.FindAsync(id);
            if (fichasMedicas == null)
            {
                return NotFound();
            }

            if (medicoId != 0)
            {
                ViewBag.usuarioId = medicoId;
                ViewBag.funcaoId = 2;
            }
            ViewData["MedicoId"] = new SelectList(_context.Usuarios.Where(u => u.FuncaoId == 2), "UsuarioId", "NomeUsuario");
            ViewData["PacienteId"] = new SelectList(_context.Usuarios.Where(u => u.FuncaoId == 1), "UsuarioId", "NomeUsuario");
            return View(fichasMedicas); // Retorna a view de edição com o modelo
        }


        // POST: FichasMedicas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FichaId,PacienteId,MedicoId,NomeCompleto,Foto,Cpf,Celular,Endereco,TextoRico")] FichasMedicas fichasMedicas, int usuarioId, IFormFile Foto)
        {
            if (id != fichasMedicas.FichaId)
            {
                return NotFound();
            }

            try
            {
                if (Foto != null && Foto.Length > 0)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Foto.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    Foto.CopyTo(new FileStream(filePath, FileMode.Create));

                    fichasMedicas.Foto = uniqueFileName; // Salve o nome do arquivo na propriedade Foto da sua classe
                }
                else
                {
                    // Mantenha a foto existente se nenhum novo arquivo for enviado
                    var existingFicha = _context.FichasMedicas.AsNoTracking().FirstOrDefault(f => f.FichaId == fichasMedicas.FichaId);
                    if (existingFicha != null)
                    {
                        fichasMedicas.Foto = existingFicha.Foto;
                    }
                }

                _context.Update(fichasMedicas);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichasMedicasExists(fichasMedicas.FichaId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ViewData["MedicoId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", fichasMedicas.MedicoId);
            ViewData["PacienteId"] = new SelectList(_context.Usuarios, "UsuarioId", "UsuarioId", fichasMedicas.PacienteId);
            // Redirecionar para a página correta com base no ID do usuário
            return RedirectToAction("Index", "FichasMedicas", new { medicoId = usuarioId, funcaoId = 2 });
        }

        // GET: FichasMedicas/Delete/5
        public async Task<IActionResult> Delete(int? id, int medicoId)
        {
            if (id == null || _context.FichasMedicas == null)
            {
                return NotFound();
            }

            var fichasMedicas = await _context.FichasMedicas
                .Include(f => f.Medico)
                .Include(f => f.Paciente)
                .FirstOrDefaultAsync(m => m.FichaId == id);
            if (fichasMedicas == null)
            {
                return NotFound();
            }

            if (medicoId != 0)
            {
                ViewBag.medicoId = medicoId;
                ViewBag.funcaoId = 2;
            }

            return View(fichasMedicas);
        }

        // POST: FichasMedicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int medicoId)
        {
            if (_context.FichasMedicas == null)
            {
                return Problem("Entity set 'HospitalContext.FichasMedicas'  is null.");
            }
            var fichasMedicas = await _context.FichasMedicas.FindAsync(id);
            if (fichasMedicas != null)
            {
                _context.FichasMedicas.Remove(fichasMedicas);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "FichasMedicas", new { medicoId = medicoId, funcaoId = 2 });
        }

        private bool FichasMedicasExists(int id)
        {
            return (_context.FichasMedicas?.Any(e => e.FichaId == id)).GetValueOrDefault();
        }
    }
}
