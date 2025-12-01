namespace AurumLab.Models
{
    public class EditarDispositivoViewModel
    {
        public int IdDispositivo { get; set; }

        public string Nome { get; set; } = string.Empty;

        public int IdTipoDispositivo { get; set; }

        public int IdLocal { get; set; }

        public DateOnly? DataUltimaManutencao { get; set; }

        // listas para armazenar o valor dos selects
        public List<TipoDispositivo> Tipos { get; set; } = new();
        public List<LocalDispositivo> Locais { get; set; } = new();
    }
}
