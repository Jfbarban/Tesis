using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Tesis.Models;
using Tesis.Models.NextCloudClient;

namespace Tesis.Controllers
{
    public class HomeController : Controller
    {
        public class ConnectionSettingsModel
        {
            public string Url { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public bool NetProxy { get; set; }
            public string ProxyIP { get; set; }
            public string ProxyPort { get; set; }
            public string UserProxy { get; set; }
            public string PassProxy { get; set; }
        }

        private static string archivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ListaSubido.txt");

        private static CloudConnectionConfig _config;

        private static NexxCloudClient cliente;

        private readonly CompressionService _compressionService;

        public HomeController(CompressionService compressionService)
        {
            _compressionService = compressionService;
            LimpiarCada24Horas();
        }

        private static bool loged;

        public IActionResult GetConnectionView()
        {
            return Json(new { success = true});
        }

        public IActionResult Connection()
        {
            return View();
        }


        public IActionResult SubirArchivo()
        {
            return View();
        }

        public IActionResult Index(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            return View();
        }

        [HttpPost]
        public IActionResult SetConnectionSettings([FromBody] ConnectionSettingsModel model)
        {
            var config = new CloudConnectionConfig
            {
                CloudUrl = model.Url,
                CloudUsername = model.User,
                CloudPassword = model.Password,
                NetProxy = model.NetProxy,
                ProxyIP = model.ProxyIP,
                ProxyPort = model.ProxyPort,
                UserProxy = model.UserProxy,
                ProxyPassword = model.PassProxy
            };

            _config = config;

            CrearCliente();

            return Json(new { success = true });
        }

        // GET: /Home/GetConnectionSettings
        [HttpGet]
        public IActionResult GetConnectionSettings()
        {
            if (_config != null)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public async void CrearCliente()
        {

            cliente = new NexxCloudClient();

            loged = await Task.Run(() => cliente.LoginAsync(_config.CloudUsername, 
                _config.CloudPassword, _config.CloudUrl, _config.NetProxy, 
                _config.ProxyIP, _config.ProxyPort, _config.UserProxy, _config.ProxyPassword));

        }

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromForm] string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                ModelState.AddModelError("folderPath", "Folder path is required");
                return Json(new { success = false });
            }

            try
            {
                string mesage = _config.CloudUrl + "/apps/files/" + folderPath.Substring(folderPath.LastIndexOf("\\") + 1) + ".zip";

                if (!EstaGuardado(mesage))
                {
                    ViewBag.Message = mesage;
                    string compressedFilePath = _compressionService.CompressFolder(folderPath);
                    await cliente.UploadFileAsync(compressedFilePath);
                    GuardarMensajeEnArchivo(mesage);
                    return Json(new { success = true, message = ViewBag.Message });
                }

                return Json(new { success = true, message = "Existe" });
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error uploading folder: {ex.Message}";
                return Json(new { success = false, message = $"Error uploading folder: {ex.Message}" });
            }
        }

        private bool EstaGuardado(string mesage)
        {
            if (System.IO.File.Exists(archivo))
            {
                List<string> lista = LeerMensajesDesdeArchivo();

                foreach(string s in lista){
                    if (s.Contains(mesage))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public IActionResult Listar()
        {
            List<string> mensajes = LeerMensajesDesdeArchivo();
            return View(mensajes);
        }

        private void GuardarMensajeEnArchivo(string mensaje)
        {
            // Verificar si el archivo existe; si no, crearlo
            if (!System.IO.File.Exists(archivo))
            {
                System.IO.File.Create(archivo).Close();
            }

            using (StreamWriter writer = new StreamWriter(archivo, true))
            {
                writer.WriteLine(mensaje);
                writer.Close();
            }
        }

        private List<string> LeerMensajesDesdeArchivo()
        {
            List<string> mensajes = new List<string>();

            // Verificar si el archivo existe; si no, retornar lista vacía
            if (!System.IO.File.Exists(archivo))
            {
                return mensajes;
            }

            using (StreamReader reader = new StreamReader(archivo))
            {
                string linea;
                while ((linea = reader.ReadLine()) != null)
                {
                    mensajes.Add(linea);
                }
                reader.Close();
            }
            return mensajes;
        }

        public IActionResult BorrarLista()
        {

            if (System.IO.File.Exists(archivo))
            {
                System.IO.File.Delete(archivo);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        static async Task LimpiarCada24Horas()
        {
            while (true)
            {
                await Task.Delay(24 * 60 * 60 * 1000);

                if (System.IO.File.Exists(archivo))
                {
                    System.IO.File.Delete(archivo);
                }

            }
        }

    }
}
