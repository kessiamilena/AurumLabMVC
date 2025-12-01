
using AurumLab.Data;
using AurumLab.Models;
using AurumLab.Services;
using Microsoft.AspNetCore.Mvc;

namespace AurumLab.Controllers
{
    public class PerfilController : Controller
    {
        private readonly AppDbContext _context;

        public PerfilController(AppDbContext context)
        {
            _context = context;
        }

        // GET tela de perfil
        public IActionResult Index()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if (usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // pega os dados por completo do usuário logado na sessão pelo id
            var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == usuarioId);

            var viewModel = new PerfilViewModel
            {
                IdUsuario = usuario.IdUsuario,
                NomeCompleto = usuario.NomeCompleto,
                // NomeUsuario = usuario.NomeUsuario,
                NomeUsuario = usuario?.NomeUsuario ?? "Usuário",
                Email = usuario.Email,
                RegraId = usuario.RegraId,

                // listando as regras que existem dentro da tabela RegraPerfil para mostrar dentro do select
                Regras = _context.RegraPerfils.ToList(),

                // se existir foto, converte a foto para string
                // se nao existir, retorna nulo
                FotoBase64 = usuario.Foto != null
                    ? Convert.ToBase64String(usuario.Foto)
                    : null
            
            };

            return View(viewModel);
        }

        // POST - Salvar dados de texto do perfil
        [HttpPost]
        public IActionResult Index(PerfilViewModel model)
        {
            var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == model.IdUsuario);

            if (usuario == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if(!string.IsNullOrWhiteSpace(model.NovaSenha))
            {
                if(model.NovaSenha != model.ConfirmarSenha)
                {
                    ViewBag.Erro = "As senhas não são iguais.";

                    // quando o POST(a atualização que estamos fazendo) dá erro e volta pra View, a lista de regras não vem preenchida.
                    // pq ela é um select com a lista de regras que estamos puxando do banco.
                    model.Regras = _context.RegraPerfils.ToList();
                    return View(model);
                }

                usuario.Senha = HashService.GerarHashBytes(model.NovaSenha);
            }

            // atualizar restante dos dados
            usuario.NomeCompleto = model.NomeCompleto;
            usuario.NomeUsuario = model.NomeUsuario;
            usuario.Email = model.Email;
            usuario.RegraId = model.RegraId;

            _context.SaveChangesAsync();

            // ViewBag morre no redirect.
            // TempData sobrevive a um redirect (uma vez).
            TempData["Sucesso"] = "Perfil atualizado com sucesso!";
            return RedirectToAction("Index");
        }

        // POST - Atualizar a foto de perfil (MODAL)
        [HttpPost]
        public IActionResult AtualizarFoto(int idUsuario, IFormFile foto)
        {
            // IFormFile -> representa um arquivo enviado pelo formulário no HTML
            // quando o formulário é enviado, o navegador envia o arquivo e o MVC converte para um objeto IFormFile.

            if (foto == null || foto.Length == 0)
            {
                return RedirectToAction("Index");
            }

            var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == idUsuario);

            if(usuario == null)
            {
                return RedirectToAction("Index", "Login");
            }

            using (var ms = new MemoryStream())
            {
                foto.CopyTo(ms);
                usuario.Foto = ms.ToArray(); //salva como VARBINARY(MAX) - até 2GB - dentro do banco
            }

            _context.SaveChanges();

            TempData["Sucesso"] = "Foto atualizada com sucesso!";
            return RedirectToAction("Index");
        }
    }
}