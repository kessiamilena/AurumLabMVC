using System;
using System.Collections.Generic;

namespace AurumLab.Models;

public partial class TipoManutencao
{
    public int IdTipoManutencao { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Manutencao> Manutencaos { get; set; } = new List<Manutencao>();
}
