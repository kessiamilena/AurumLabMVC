using System;
using System.Collections.Generic;

namespace AurumLab.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NomeCompleto { get; set; } = null!;

    public string? NomeUsuario { get; set; }

    public string Email { get; set; } = null!;

    public byte[] Senha { get; set; } = null!;

    public byte[]? Foto { get; set; }

    public DateTime CriadoEm { get; set; }

    public int RegraId { get; set; }

    public virtual ICollection<Manutencao> Manutencaos { get; set; } = new List<Manutencao>();

    public virtual RegraPerfil Regra { get; set; } = null!;
}
