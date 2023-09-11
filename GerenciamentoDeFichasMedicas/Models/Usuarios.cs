using System;
using System.Collections.Generic;

namespace GerenciamentoDeFichasMedicas.Models;

public partial class Usuarios
{
    public int UsuarioId { get; set; }

    public string NomeUsuario { get; set; } = null!;

    public string SenhaHash { get; set; } = null!;

    public int? FuncaoId { get; set; }

    public virtual ICollection<FichasMedicas> FichasMedicaMedicos { get; set; } = new List<FichasMedicas>();

    public virtual ICollection<FichasMedicas> FichasMedicaPacientes { get; set; } = new List<FichasMedicas>();

    public virtual Funcoes? Funcao { get; set; }
}
