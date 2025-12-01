
namespace AurumLab.Models
{
    public class PerfilViewModel
    {
        public int IdUsuario {get; set;}
        public string NomeCompleto {get; set;}
        public string? NomeUsuario {get; set;}
        public string Email {get; set;}
        public int RegraId {get; set;}
        public List<RegraPerfil> Regras {get; set;}

        public string? NovaSenha {get; set;}
        public string? ConfirmarSenha {get; set;}

        public string? FotoBase64 {get; set;}

        public string? FotoFinal => 
                FotoBase64 != null
                ? $"data:image/*;base64,{FotoBase64}"
                : "/assets/img/img-perfil.png";
    }
}