using GerenciamentoDeFichasMedicas.Models;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoDeFichasMedicas.Controllers
{
    public class ViewModelsController : Controller
    {
        private readonly HospitalContext _context;

        public ViewModelsController(HospitalContext context)
        {
            _context = context;
        }

        public IActionResult GetFormData()
        {
            var funcoes = _context.Funcoes.ToList();
            return Json(funcoes);
        }
    }

}
