using System;
using System.Collections.Generic;

namespace AurumLab.Models;

public partial class TipoDispositivo
{
    public int IdTipoDispositivo { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Dispositivo> Dispositivos { get; set; } = new List<Dispositivo>();
}
