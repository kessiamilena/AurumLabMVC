
using AurumLab.Data;
using AurumLab.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AurumLab.Controllers
{
    public class DispositivosController : Controller
    {
        private readonly AppDbContext _context;

        public DispositivosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? busca = null, int? tipoId = null, int? localId = null)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if(usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // u: palavrinha auxiliar para representar usuário no banco.
            var usuario = _context.Usuarios.FirstOrDefault(u => u.IdUsuario == usuarioId);

            // Bloqueia o acesso se não for professor (regra 2)
            // Se não for regra 2, não deixa o usuário acessar e volta para a tela de dashboard
            if(usuario.RegraId != 2)
            {
                TempData["Erro"] = "Acesso permitido somente para professores.";
                return RedirectToAction("Dashboard", "Dashboard");
            }

            // select dentro de dispositivos
            var selectBusca = _context.Dispositivos
                // include funciona como join, mas de forma mais simples.
                // uso o join quando quero criar agrupamentos de itens, por exemplo
                .Include(dispositivo => dispositivo.IdTipoDispositivoNavigation)
                .Include(dispositivo => dispositivo.IdLocalNavigation)
                .AsQueryable(); // deixa a consulta aberta para continuar sendo montada. Agora podemos colocar Where, por exemplo

            // Filtros
            if(!string.IsNullOrWhiteSpace(busca))
            {
                // select de busca vai armazenar agora o valor buscado no banco
                // where verifica se existe dispositivos com o nome digitado na busca
                // Contains - verifica se contem esse valor no banco.
                selectBusca = selectBusca.Where(dispositivo => dispositivo.Nome.Contains(busca));
            }

            // verifica se existe algum valor dentro do tipoId
            if(tipoId.HasValue)
            {
                // verifica se o id do tipo do select selecionado é igual ao tipo cadastrado no banco para o dispositivo
                selectBusca = selectBusca.Where(dispositivo => dispositivo.IdTipoDispositivo == tipoId.Value);
            }

            if(localId.HasValue)
            {
                selectBusca = selectBusca.Where(dispositivo => dispositivo.IdLocal == localId.Value);
            }

            DispositivosViewModel viewModel = new DispositivosViewModel
            {
                // se não existir nome de usuário no banco, ele passa o nome "Usuário".
                NomeUsuario = usuario.NomeUsuario ?? "Usuário",
                FotoUsuario = usuario.Foto != null
                    ? $"data:image/*;base64,{Convert.ToBase64String(usuario.Foto)}"
                    : "/assets/img/img-perfil.png",
                
                // puxa os dispositivos da model dispositivos (todos os dispositivos do banco) e mostra na tela de acordo com o selectBusca
                Dispositivos = selectBusca.ToList(),

                // lista todos os tipos e locais que existem na tabela de tipos e locais do banco para mostrar dentro dos selects
                Tipos = _context.TipoDispositivos.ToList(),
                Locais = _context.LocalDispositivos.ToList(),

                Busca = busca,
                TipoIdSelecionado = tipoId,
                LocalIdSelecionado = localId

            };

            return View(viewModel);

        }

        [HttpGet] // mostra somente a visualização do dispositivo pelo id
        public IActionResult Editar(int id)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if(usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var dispositivo = _context.Dispositivos.FirstOrDefault(d => d.IdDispositivo == id);

            if(dispositivo == null)
            {
                return NotFound(); // retorna não encontrado
            }

            EditarDispositivoViewModel vm = new EditarDispositivoViewModel
            {
                IdDispositivo = dispositivo.IdDispositivo,
                Nome = dispositivo.Nome,
                IdTipoDispositivo = dispositivo.IdTipoDispositivo,
                IdLocal = dispositivo.IdLocal,
                DataUltimaManutencao = dispositivo.DataUltimaManutencao,

                Tipos = _context.TipoDispositivos.ToList(),
                Locais = _context.LocalDispositivos.ToList()
            };

            return View("Editar", vm);
        }

        // editar dispositivo de acordo com o dispositivo puxado da view model
        [HttpPost]
        public IActionResult Editar(EditarDispositivoViewModel vm)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            if(usuarioId == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var dispositivo = _context.Dispositivos.FirstOrDefault(d => d.IdDispositivo == vm.IdDispositivo);

            if(dispositivo == null)
            {
                return NotFound();
            }

            dispositivo.Nome = vm.Nome;
            dispositivo.IdTipoDispositivo = vm.IdTipoDispositivo;
            dispositivo.IdLocal = vm.IdLocal;
            dispositivo.DataUltimaManutencao = vm.DataUltimaManutencao;

            _context.SaveChangesAsync();

            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public IActionResult Excluir(int id)
        {
            var dispositivo = _context.Dispositivos.FirstOrDefault(d => d.IdDispositivo == id);

            if(dispositivo == null)
            {
                return NotFound();
            }

            _context.Dispositivos.Remove(dispositivo);
            _context.SaveChangesAsync();

            TempData["Sucesso"] = "Dispositivo excluído com sucesso!";
            return RedirectToAction("Index");
        }
    }
}