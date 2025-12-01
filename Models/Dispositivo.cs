using System;
using System.Collections.Generic;

namespace AurumLab.Models;

public partial class Dispositivo
{
    public int IdDispositivo { get; set; }

    public string Nome { get; set; } = null!;

    // traz o número do dispositivo - ex. 1, 2, 50
    public int IdTipoDispositivo { get; set; }

    public int IdLocal { get; set; }

    public int NumeroDispositivo { get; set; }

    public string SituacaoOperacional { get; set; } = null!;

    public DateOnly? DataUltimaManutencao { get; set; }

    public string? Observacoes { get; set; }

    public DateTime CriadoEm { get; set; }

    public virtual LocalDispositivo IdLocalNavigation { get; set; } = null!;
    
    // objeto que possui relacionamento com esse dispositivo na tabela da foreign key
    public virtual TipoDispositivo IdTipoDispositivoNavigation { get; set; } = null!;

    public virtual ICollection<Manutencao> Manutencaos { get; set; } = new List<Manutencao>();
}
