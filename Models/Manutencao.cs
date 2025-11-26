using System;
using System.Collections.Generic;

namespace AurumLab.Models;

public partial class Manutencao
{
    public int IdManutencao { get; set; }

    public int IdDispositivo { get; set; }

    public int IdTipoManutencao { get; set; }

    public string StatusManutencao { get; set; } = null!;

    public DateOnly? DataAgendada { get; set; }

    public DateOnly? DataRealizada { get; set; }

    public string? Responsavel { get; set; }

    public string? Observacoes { get; set; }

    public int? Criado_por { get; set; }

    public DateTime Criado_em { get; set; }

    public virtual Usuario? Criado_porNavigation { get; set; }

    public virtual Dispositivo IdDispositivoNavigation { get; set; } = null!;

    public virtual TipoManutencao IdTipoManutencaoNavigation { get; set; } = null!;
}
