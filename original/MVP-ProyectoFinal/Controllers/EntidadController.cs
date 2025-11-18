using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVP_ProyectoFinal.Models;
using System.Text.Json;

namespace MVP_ProyectoFinal.Controllers
{
    [Authorize]
    public class EntidadController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("EntidadGanado") == "true" && TempData["MensajeVictoria"] == null)
            {
                var nombreSecreto = HttpContext.Session.GetString("EntidadSecreta");
                TempData["MensajeVictoria"] = "You guessed the entity! It was: " + nombreSecreto;
            }
            else
            {
                var entidadSecretaActual = HttpContext.Session.GetString("EntidadSecreta");
                if (string.IsNullOrEmpty(entidadSecretaActual))
                {
                    var entidadSecretaNueva = RepositorioEntidades.ObtenerEntidadAleatoria();
                    if (entidadSecretaNueva != null)
                    {
                        HttpContext.Session.SetString("EntidadSecreta", entidadSecretaNueva.Nombre);
                        HttpContext.Session.SetString("IntentosEntidad", "[]");
                    }
                }
            }
            var intentosJson = HttpContext.Session.GetString("IntentosEntidad") ?? "[]";
            var intentos = JsonSerializer.Deserialize<List<ResultadoIntentoEntidadVM>>(intentosJson);
            return View(intentos);
        }

        [HttpPost]
        public IActionResult Adivinar(string nombreEntidad)
        {
            var nombreSecreto = HttpContext.Session.GetString("EntidadSecreta");
            if (string.IsNullOrEmpty(nombreSecreto)) return RedirectToAction("Reiniciar");

            var intentosJson = HttpContext.Session.GetString("IntentosEntidad") ?? "[]";
            var todosLosIntentos = JsonSerializer.Deserialize<List<ResultadoIntentoEntidadVM>>(intentosJson) ?? new List<ResultadoIntentoEntidadVM>();

            if (todosLosIntentos.Any(intento => intento.NombreEntidad.Equals(nombreEntidad, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Error"] = $"You already tried '{nombreEntidad}'. Try another entity.";
                return RedirectToAction("Index");
            }

            var entidadIntentada = RepositorioEntidades.ObtenerPorNombre(nombreEntidad);
            if (entidadIntentada == null)
            {
                TempData["Error"] = "That entity does not exist in the game. Try another entity.";
                return RedirectToAction("Index");
            }

            var entidadSecreta = RepositorioEntidades.ObtenerPorNombre(nombreSecreto);
            if (entidadSecreta == null)
            {
                TempData["Error"] = "Critical error. Starting again.";
                return RedirectToAction("Reiniciar");
            }

            var longitudNombreIntentado = entidadIntentada.Nombre.Replace(" ", "").Length;
            var longitudNombreSecreto = entidadSecreta.Nombre.Replace(" ", "").Length;
            var coincideInicial = entidadIntentada.Nombre.Length > 0
                && entidadSecreta.Nombre.Length > 0
                && char.ToUpperInvariant(entidadIntentada.Nombre[0]) == char.ToUpperInvariant(entidadSecreta.Nombre[0]);

            var colorTipo = entidadIntentada.Tipo == entidadSecreta.Tipo ? "verde" : "rojo";
            var colorVida = entidadIntentada.Vida == entidadSecreta.Vida ? "verde" : "rojo";
            var colorAtaque = entidadIntentada.Ataque == entidadSecreta.Ataque ? "verde" : "rojo";
            var colorDimension = entidadIntentada.Dimension == entidadSecreta.Dimension ? "verde" : "rojo";
            var colorAnio = entidadIntentada.YearLanzamiento == entidadSecreta.YearLanzamiento ? "verde" : "rojo";
            var colorLongitud = longitudNombreIntentado == longitudNombreSecreto ? "verde" : "rojo";
            var colorInicial = coincideInicial ? "verde" : "rojo";

            var hintVida = string.Empty;
            if (entidadIntentada.Vida != entidadSecreta.Vida)
            {
                if (entidadIntentada.Vida < entidadSecreta.Vida) hintVida = " ▲ more health";
                else if (entidadIntentada.Vida > entidadSecreta.Vida) hintVida = " ▼ less health";
            }

            var hintAtaque = string.Empty;
            if (entidadIntentada.Ataque != entidadSecreta.Ataque)
            {
                if (entidadIntentada.Ataque < entidadSecreta.Ataque) hintAtaque = " ▲ more attack";
                else if (entidadIntentada.Ataque > entidadSecreta.Ataque) hintAtaque = " ▼ less attack";
            }

            var hintAnio = string.Empty;
            if (entidadIntentada.YearLanzamiento != entidadSecreta.YearLanzamiento)
            {
                if (entidadIntentada.YearLanzamiento < entidadSecreta.YearLanzamiento) hintAnio = " ▲ newer version";
                else if (entidadIntentada.YearLanzamiento > entidadSecreta.YearLanzamiento) hintAnio = " ▼ older version";
            }

            var hintLongitud = string.Empty;
            if (longitudNombreIntentado < longitudNombreSecreto) hintLongitud = " ▲ longer name";
            else if (longitudNombreIntentado > longitudNombreSecreto) hintLongitud = " ▼ shorter name";

            var resultado = new ResultadoIntentoEntidadVM
            {
                NombreEntidad = entidadIntentada.Nombre,
                Tipo = entidadIntentada.Tipo,
                Vida = entidadIntentada.Vida,
                Ataque = entidadIntentada.Ataque,
                Dimension = entidadIntentada.Dimension,
                YearLanzamiento = entidadIntentada.YearLanzamiento,
                LongitudNombre = longitudNombreIntentado,
                CoincideInicial = coincideInicial ? "Yes" : "No",
                ColorLongitudNombre = colorLongitud,
                ColorCoincideInicial = colorInicial,
                HintLongitudNombre = hintLongitud,
                ColorTipo = colorTipo,
                ColorVida = colorVida,
                ColorAtaque = colorAtaque,
                ColorDimension = colorDimension,
                ColorAnio = colorAnio,
                HintVida = hintVida,
                HintAtaque = hintAtaque,
                HintAnio = hintAnio
            };

            todosLosIntentos.Add(resultado);
            HttpContext.Session.SetString("IntentosEntidad", JsonSerializer.Serialize(todosLosIntentos));

            if (entidadIntentada.Nombre.Equals(entidadSecreta.Nombre, StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensajeVictoria"] = $"You guessed the entity! It was: {entidadSecreta.Nombre}";
                HttpContext.Session.SetString("EntidadGanado", "true");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult GiveUp()
        {
            var nombreSecreto = HttpContext.Session.GetString("EntidadSecreta");
            if (string.IsNullOrEmpty(nombreSecreto)) return RedirectToAction("Reiniciar");

            HttpContext.Session.SetString("EntidadGanado", "true");
            TempData["MensajeVictoria"] = "You gave up. The entity was: " + nombreSecreto;
            return RedirectToAction("Index");
        }

        public IActionResult Reiniciar()
        {
            HttpContext.Session.Remove("EntidadSecreta");
            HttpContext.Session.Remove("IntentosEntidad");
            HttpContext.Session.Remove("EntidadGanado");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SugerirEntidades(string term)
        {
            if (string.IsNullOrEmpty(term)) return Json(new List<string>());
            var todasLasEntidades = RepositorioEntidades.ObtenerTodos();
            var sugerencias = todasLasEntidades
                .Where(e => e.Nombre.StartsWith(term, StringComparison.OrdinalIgnoreCase))
                .Select(e => e.Nombre)
                .Take(5)
                .ToList();
            return Json(sugerencias);
        }
    }
}