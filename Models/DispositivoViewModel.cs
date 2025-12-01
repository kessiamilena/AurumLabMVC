namespace AurumLab.Models
{
    public class DispositivosViewModel
    {
        public string NomeUsuario { get; set; } = string.Empty;
        public string FotoUsuario { get; set; } = string.Empty;

        // filtros
        public string? Busca { get; set; }
        public int? TipoIdSelecionado { get; set; }
        public int? LocalIdSelecionado { get; set; }

        public List<TipoDispositivo> Tipos { get; set; } = new();
        public List<LocalDispositivo> Locais { get; set; } = new();

        // AQUI: usa direto a model verdadeira
        public List<Dispositivo> Dispositivos { get; set; } = new();
    }
}
