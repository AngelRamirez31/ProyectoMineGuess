using Microsoft.AspNetCore.Mvc;
using MVP_ProyectoFinal.Models;
using System.Text.Json;

namespace MVP_ProyectoFinal.Controllers
{
    public class JuegoController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("JuegoGanado") == "true" && TempData["MensajeVictoria"] == null)
            {
                var nombreSecreto = HttpContext.Session.GetString("BloqueSecreto");
                TempData["MensajeVictoria"] = "You guessed the block! It was: " + nombreSecreto;
            }
            else
            {
                var bloqueSecretoActual = HttpContext.Session.GetString("BloqueSecreto");
                if (string.IsNullOrEmpty(bloqueSecretoActual))
                {
                    var bloqueSecretoNuevo = RepositorioBloques.ObtenerBloqueAleatorio();
                    if (bloqueSecretoNuevo != null)
                    {
                        HttpContext.Session.SetString("BloqueSecreto", bloqueSecretoNuevo.Nombre);
                        HttpContext.Session.SetString("Intentos", "[]");
                    }
                }
            }
            var intentosJson = HttpContext.Session.GetString("Intentos") ?? "[]";
            var intentos = JsonSerializer.Deserialize<List<ResultadoIntentoVM>>(intentosJson);
            return View(intentos);
        }

        [HttpPost]
        public IActionResult Adivinar(string nombreBloque)
        {
            var nombreSecreto = HttpContext.Session.GetString("BloqueSecreto");
            if (string.IsNullOrEmpty(nombreSecreto)) return RedirectToAction("Reiniciar");

            var intentosJson = HttpContext.Session.GetString("Intentos") ?? "[]";
            var todosLosIntentos = JsonSerializer.Deserialize<List<ResultadoIntentoVM>>(intentosJson) ?? new List<ResultadoIntentoVM>();

            if (todosLosIntentos.Any(intento => intento.NombreBloque.Equals(nombreBloque, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Error"] = $"You already tried '{nombreBloque}'. Try another block.";
                return RedirectToAction("Index");
            }

            var bloqueIntentado = RepositorioBloques.ObtenerPorNombre(nombreBloque);
            if (bloqueIntentado == null)
            {
                TempData["Error"] = "That block does not exist in the game. Try another block.";
                return RedirectToAction("Index");
            }

            var bloqueSecreto = RepositorioBloques.ObtenerPorNombre(nombreSecreto);
            if (bloqueSecreto == null)
            {
                TempData["Error"] = "Critical error. Starting again.";
                return RedirectToAction("Reiniciar");
            }

            var valorVersionIntentada = VersionComparer.ObtenerValor(bloqueIntentado.Version);
            var valorVersionSecreta = VersionComparer.ObtenerValor(bloqueSecreto.Version);

            var longitudNombreIntentado = bloqueIntentado.Nombre.Replace(" ", "").Length;
            var longitudNombreSecreto = bloqueSecreto.Nombre.Replace(" ", "").Length;
            var coincideInicial = bloqueIntentado.Nombre.Length > 0
                && bloqueSecreto.Nombre.Length > 0
                && char.ToUpperInvariant(bloqueIntentado.Nombre[0]) == char.ToUpperInvariant(bloqueSecreto.Nombre[0]);

            var colorVersion = string.Equals(bloqueIntentado.Version.Trim(), bloqueSecreto.Version.Trim(), StringComparison.OrdinalIgnoreCase) ? "verde" : "rojo";
            var colorBioma = bloqueIntentado.Bioma == bloqueSecreto.Bioma ? "verde" : "rojo";
            var colorCrafteable = bloqueIntentado.EsCrafteable == bloqueSecreto.EsCrafteable ? "verde" : "rojo";
            var colorExterior = bloqueIntentado.EsDeExterior == bloqueSecreto.EsDeExterior ? "verde" : "rojo";
            var colorLongitud = longitudNombreIntentado == longitudNombreSecreto ? "verde" : "rojo";
            var colorInicial = coincideInicial ? "verde" : "rojo";
            var colorAnio = bloqueIntentado.YearLanzamiento == bloqueSecreto.YearLanzamiento ? "verde" : "rojo";

            var hintVersion = string.Empty;
            if (!string.Equals(bloqueIntentado.Version.Trim(), bloqueSecreto.Version.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                if (valorVersionIntentada < valorVersionSecreta) hintVersion = " ▲ newer version";
                else if (valorVersionIntentada > valorVersionSecreta) hintVersion = " ▼ older version";
            }

            var colorFuncion = string.Empty;
            var hintFuncion = string.Empty;
            if (!string.IsNullOrWhiteSpace(bloqueIntentado.Funcion) && !string.IsNullOrWhiteSpace(bloqueSecreto.Funcion))
            {
                if (string.Equals(bloqueIntentado.Funcion.Trim(), bloqueSecreto.Funcion.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    colorFuncion = "verde";
                }
                else
                {
                    colorFuncion = "rojo";
                }
                hintFuncion = bloqueIntentado.Funcion;
            }
            else if (!string.IsNullOrWhiteSpace(bloqueIntentado.Funcion))
            {
                hintFuncion = bloqueIntentado.Funcion;
            }

            var hintLongitud = string.Empty;
            if (longitudNombreIntentado < longitudNombreSecreto) hintLongitud = " ▲ longer name";
            else if (longitudNombreIntentado > longitudNombreSecreto) hintLongitud = " ▼ shorter name";

            var hintAnio = string.Empty;
            if (bloqueIntentado.YearLanzamiento != bloqueSecreto.YearLanzamiento)
            {
                if (bloqueIntentado.YearLanzamiento < bloqueSecreto.YearLanzamiento) hintAnio = " ▲ newer version";
                else if (bloqueIntentado.YearLanzamiento > bloqueSecreto.YearLanzamiento) hintAnio = " ▼ older version";
            }

            var resultado = new ResultadoIntentoVM
            {
                NombreBloque = bloqueIntentado.Nombre,
                Version = bloqueIntentado.Version,
                Funcion = bloqueIntentado.Funcion,
                Bioma = bloqueIntentado.Bioma,
                EsCrafteable = bloqueIntentado.EsCrafteable ? "Yes" : "No",
                EsDeExterior = bloqueIntentado.EsDeExterior ? "Yes" : "No",
                YearLanzamiento = bloqueIntentado.YearLanzamiento,
                LongitudNombre = longitudNombreIntentado,
                CoincideInicial = coincideInicial ? "Yes" : "No",
                ColorLongitudNombre = colorLongitud,
                ColorCoincideInicial = colorInicial,
                HintLongitudNombre = hintLongitud,
                ColorVersion = colorVersion,
                ColorFuncion = colorFuncion,
                ColorBioma = colorBioma,
                ColorCrafteable = colorCrafteable,
                ColorExterior = colorExterior,
                ColorAnio = colorAnio,
                HintVersion = hintVersion,
                HintFuncion = hintFuncion,
                HintAnio = hintAnio
            };

            todosLosIntentos.Add(resultado);
            HttpContext.Session.SetString("Intentos", JsonSerializer.Serialize(todosLosIntentos));

            if (bloqueIntentado.Nombre.Equals(bloqueSecreto.Nombre, StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensajeVictoria"] = $"You guessed the block! It was: {bloqueSecreto.Nombre}";
                HttpContext.Session.SetString("JuegoGanado", "true");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult GiveUp()
        {
            var nombreSecreto = HttpContext.Session.GetString("BloqueSecreto");
            if (string.IsNullOrEmpty(nombreSecreto)) return RedirectToAction("Reiniciar");

            HttpContext.Session.SetString("JuegoGanado", "true");
            TempData["MensajeVictoria"] = "You gave up. The block was: " + nombreSecreto;
            return RedirectToAction("Index");
        }

        public IActionResult Reiniciar()
        {
            HttpContext.Session.Remove("BloqueSecreto");
            HttpContext.Session.Remove("Intentos");
            HttpContext.Session.Remove("JuegoGanado");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SugerirBloques(string term)
        {
            if (string.IsNullOrEmpty(term)) return Json(new List<string>());
            var todosLosBloques = RepositorioBloques.ObtenerTodos();
            var sugerencias = todosLosBloques
                .Where(b => b.Nombre.StartsWith(term, StringComparison.OrdinalIgnoreCase))
                .Select(b => b.Nombre)
                .Take(5)
                .ToList();
            return Json(sugerencias);
        }
    }
}