using System;
using System.Collections.Generic;

namespace GerenciamentoDeFichasMedicas.Models;

public partial class Funcoes
{
    public int FuncaoId { get; set; }

    public string NomeFuncao { get; set; } = null!;

    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
