
using AurumLab.Data;
using Microsoft.AspNetCore.Mvc;

namespace AurumLab.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            // valida se existe login realizado
            if(HttpContext.Session.GetInt32("UsuarioId") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.IdUsuario == usuarioId);

            // TIPOS DISPOSITIVO - JOIN + AGRUPAMENTO
            // consultar a tabela dispositivos através da ViewModel

            // SELECT * FROM Dispositivos
            var dispositivosPorTipo = _context.Dispositivos
            .Join(
                    _context.TipoDispositivos, // JOIN TipoDispositivos
                    dispositivo => dispositivo.IdTipoDispositivo, // ON dispositivo.IdTipoDispositivo
                    tipoDispositivo => tipoDispositivo.IdTipoDispositivo, // = tipoDispositivo.IdTipoDispositivo
                    (dispositivo, tipoDispositivo) => new {dispositivo, tipoDispositivo.Nome}

                    // Para cada par encontrado - um dispositivo e seu tipoDispositivo correspondente - monta um objeto:
                    // dispositivo -> objeto completo
                    // nome -> o nome do tipo do dispositivo
                    // { 
                    //      dispositivo = {objeto Dispositivo inteiro}
                    //      Nome = "Sensor" (exemplo)
                    // }
                )

            var dispositivosPorTipo = _context.Dispositivos
            .Join(
                    _context.TipoDispositivos, 
                    dispositivo => dispositivo.IdTipoDispositivo, 
                    tipoDispositivo => tipoDispositivo.IdTipoDispositivo, 
                    (dispositivo, tipoDispositivo) => new {dispositivo, tipoDispositivo.Nome}

                )

                // SELECT * FROM Dispositivos d
                // JOIN TipoDispositivos td
                // ON d.IdTipoDispositivo = td.IdTipoDispositivo

                // 4 linha -> cria objeto
                // {
                //      dispositivo = {nome: "xyz", "local": "sala 10"}
                //      Nome = "notebook"
                // }

        }
    }
}



