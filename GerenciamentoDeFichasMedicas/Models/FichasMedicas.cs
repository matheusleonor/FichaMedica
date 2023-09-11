using System;
using System.Collections.Generic;

namespace GerenciamentoDeFichasMedicas.Models;

public partial class FichasMedicas
{
    public int FichaId { get; set; }

    public int? PacienteId { get; set; }

    public int? MedicoId { get; set; }

    public string NomeCompleto { get; set; } = null!;

    public string? Foto { get; set; }

    public string Cpf { get; set; } = null!;

    public string Celular { get; set; } = null!;

    public string? Endereco { get; set; }

    public string? TextoRico { get; set; }

    public virtual Usuarios? Medico { get; set; }

    public virtual Usuarios? Paciente { get; set; }
}
