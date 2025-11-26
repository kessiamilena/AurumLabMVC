

using AurumLab.Data;
using AurumLab.Models;
using AurumLab.Services;
using Microsoft.AspNetCore.Mvc;

namespace AurumLab.Controllers
{
    public class CadastroController : Controller
    {
        private readonly AppDbContext _context;

        public CadastroController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Criar(string nome, string email, string senha, string confirmar)
        {
            if( string.IsNullOrWhiteSpace(nome) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(senha) ||
                string.IsNullOrWhiteSpace(confirmar))
            {
                ViewBag.Erro = "Preencha todos os campos";
                return View("Index");
            }

            if(senha != confirmar)
            {
                ViewBag.Erro = "As senhas não conferem.";
                return View("Index");
            }

            // verifica se o e-mail já está cadastrado
            // Any() é parecido com o FirstOrDefault()
            // Diferença: FirstOrDefault traz o objeto por completo - ex: nome, foto
            // Any() - serve só para validar se existe esse e-mail
            if(_context.Usuarios.Any(usuario => usuario.Email == email))
            {
                // auxiliar usuario percorre os usuários procurando pelo e-mail até encontrar o e-mail igual ao digitado no input.
                ViewBag.Erro = "E-mail já cadastrado";
                return View("Index");
            }

            byte[] hash = HashService.GerarHashBytes(senha);

            Usuario usuario = new Usuario
            {
                NomeCompleto = nome,
                Email = email,
                Senha = hash,
                Foto = null,
                RegraId = 1 // aluno por padrão
            };

            // salvar no banco
            // add é igual ao insert no banco
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            // redireciona para o login
            return RedirectToAction("Index", "Login");


        }
    }
}