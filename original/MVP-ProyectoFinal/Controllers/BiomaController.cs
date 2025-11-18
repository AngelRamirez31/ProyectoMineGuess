using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVP_ProyectoFinal.Models;
using System.Text.Json;

namespace MVP_ProyectoFinal.Controllers
{
    [Authorize]
    public class BiomaController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("BiomaGanado") == "true" && TempData["MensajeVictoria"] == null)
            {
                var nombreSecreto = HttpContext.Session.GetString("BiomaSecreto");
                TempData["MensajeVictoria"] = "You guessed the biome! It was: " + nombreSecreto;
            }
            else
            {
                var biomaSecretoActual = HttpContext.Session.GetString("BiomaSecreto");
                if (string.IsNullOrEmpty(biomaSecretoActual))
                {
                    var biomaSecretoNuevo = RepositorioBiomas.ObtenerBiomaAleatorio();
                    if (biomaSecretoNuevo != null)
                    {
                        HttpContext.Session.SetString("BiomaSecreto", biomaSecretoNuevo.Nombre);
                        HttpContext.Session.SetString("IntentosBioma", "[]");
                    }
                }
            }
            var intentosJson = HttpContext.Session.GetString("IntentosBioma") ?? "[]";
            var intentos = JsonSerializer.Deserialize<List<ResultadoIntentoBiomaVM>>(intentosJson);
            return View(intentos);
        }

        [HttpPost]
        public IActionResult Adivinar(string nombreBioma)
        {
            var nombreSecreto = HttpContext.Session.GetString("BiomaSecreto");
            if (string.IsNullOrEmpty(nombreSecreto)) return RedirectToAction("Reiniciar");

            var intentosJson = HttpContext.Session.GetString("IntentosBioma") ?? "[]";
            var todosLosIntentos = JsonSerializer.Deserialize<List<ResultadoIntentoBiomaVM>>(intentosJson) ?? new List<ResultadoIntentoBiomaVM>();

            if (todosLosIntentos.Any(intento => intento.NombreBioma.Equals(nombreBioma, StringComparison.OrdinalIgnoreCase)))
            {
                TempData["Error"] = $"You already tried '{nombreBioma}'. Try another biome.";
                return RedirectToAction("Index");
            }

            var biomaIntentado = RepositorioBiomas.ObtenerPorNombre(nombreBioma);
            if (biomaIntentado == null)
            {
                TempData["Error"] = "That biome does not exist in the game. Try another biome.";
                return RedirectToAction("Index");
            }

            var biomaSecreto = RepositorioBiomas.ObtenerPorNombre(nombreSecreto);
            if (biomaSecreto == null)
            {
                TempData["Error"] = "Critical error. Starting again.";
                return RedirectToAction("Reiniciar");
            }

            var longitudNombreIntentado = biomaIntentado.Nombre.Replace(" ", "").Length;
            var longitudNombreSecreto = biomaSecreto.Nombre.Replace(" ", "").Length;
            var coincideInicial = biomaIntentado.Nombre.Length > 0 && biomaSecreto.Nombre.Length > 0 &&
                char.ToUpperInvariant(biomaIntentado.Nombre[0]) == char.ToUpperInvariant(biomaSecreto.Nombre[0]);

            var colorClima = biomaIntentado.Clima == biomaSecreto.Clima ? "verde" : "rojo";
            var colorPrecipitacion = biomaIntentado.Precipitacion == biomaSecreto.Precipitacion ? "verde" : "rojo";
            var colorDimension = biomaIntentado.Dimension == biomaSecreto.Dimension ? "verde" : "rojo";
            var colorAltura = biomaIntentado.Altura == biomaSecreto.Altura ? "verde" : "rojo";
            var colorAnio = biomaIntentado.YearLanzamiento == biomaSecreto.YearLanzamiento ? "verde" : "rojo";

            var hintClima = string.Empty;
            if (biomaIntentado.Clima != biomaSecreto.Clima)
            {
                var catIntentado = CategoriaClima(biomaIntentado.Clima);
                var catSecreto = CategoriaClima(biomaSecreto.Clima);
                if (catIntentado < catSecreto) hintClima = " ▲ warmer";
                else if (catIntentado > catSecreto) hintClima = " ▼ colder";
            }

            var hintPrecipitacion = string.Empty;
            if (biomaIntentado.Precipitacion != biomaSecreto.Precipitacion)
            {
                if (biomaSecreto.Precipitacion == "None") hintPrecipitacion = " (the secret biome has almost no rain)";
                else if (biomaSecreto.Precipitacion == "Rain") hintPrecipitacion = " (it rains in the secret biome)";
                else if (biomaSecreto.Precipitacion == "Snow") hintPrecipitacion = " (it snows in the secret biome)";
            }

            var hintAltura = string.Empty;
            if (biomaIntentado.Altura != biomaSecreto.Altura)
            {
                var altIntentado = NivelAltura(biomaIntentado.Altura);
                var altSecreto = NivelAltura(biomaSecreto.Altura);
                if (altIntentado < altSecreto) hintAltura = " ▲ higher";
                else if (altIntentado > altSecreto) hintAltura = " ▼ lower";
            }

            var hintAnio = string.Empty;
            if (biomaIntentado.YearLanzamiento < biomaSecreto.YearLanzamiento) hintAnio = " ▲ newer version";
            else if (biomaIntentado.YearLanzamiento > biomaSecreto.YearLanzamiento) hintAnio = " ▼ older version";

            var hintLongitud = string.Empty;
            if (longitudNombreIntentado < longitudNombreSecreto) hintLongitud = " ▲ longer name";
            else if (longitudNombreIntentado > longitudNombreSecreto) hintLongitud = " ▼ shorter name";

            var resultado = new ResultadoIntentoBiomaVM
            {
                NombreBioma = biomaIntentado.Nombre,
                Clima = biomaIntentado.Clima,
                Precipitacion = biomaIntentado.Precipitacion,
                Dimension = biomaIntentado.Dimension,
                Altura = biomaIntentado.Altura,
                YearLanzamiento = biomaIntentado.YearLanzamiento,
                VersionLanzamiento = biomaIntentado.VersionLanzamiento,
                LongitudNombre = longitudNombreIntentado,
                CoincideInicial = coincideInicial ? "Yes" : "No",
                ColorLongitudNombre = longitudNombreIntentado == longitudNombreSecreto ? "verde" : "rojo",
                ColorCoincideInicial = coincideInicial ? "verde" : "rojo",
                HintLongitudNombre = hintLongitud,
                ColorClima = colorClima,
                ColorPrecipitacion = colorPrecipitacion,
                ColorDimension = colorDimension,
                ColorAltura = colorAltura,
                ColorAnio = colorAnio,
                HintClima = hintClima,
                HintPrecipitacion = hintPrecipitacion,
                HintAltura = hintAltura,
                HintAnio = hintAnio
            };

            todosLosIntentos.Add(resultado);
            HttpContext.Session.SetString("IntentosBioma", JsonSerializer.Serialize(todosLosIntentos));

            if (biomaIntentado.Nombre.Equals(biomaSecreto.Nombre, StringComparison.OrdinalIgnoreCase))
            {
                TempData["MensajeVictoria"] = $"You guessed the biome! It was: {biomaSecreto.Nombre}";
                HttpContext.Session.SetString("BiomaGanado", "true");
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult GiveUp()
        {
            var nombreSecreto = HttpContext.Session.GetString("BiomaSecreto");
            if (string.IsNullOrEmpty(nombreSecreto)) return RedirectToAction("Reiniciar");

            HttpContext.Session.SetString("BiomaGanado", "true");
            TempData["MensajeVictoria"] = "You gave up. The biome was: " + nombreSecreto;
            return RedirectToAction("Index");
        }

        public IActionResult Reiniciar()
        {
            HttpContext.Session.Remove("BiomaSecreto");
            HttpContext.Session.Remove("IntentosBioma");
            HttpContext.Session.Remove("BiomaGanado");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult SugerirBiomas(string term)
        {
            if (string.IsNullOrEmpty(term)) return Json(new List<string>());
            var todosLosBiomas = RepositorioBiomas.ObtenerTodos();
            var sugerencias = todosLosBiomas
                .Where(b => b.Nombre.StartsWith(term, StringComparison.OrdinalIgnoreCase))
                .Select(b => b.Nombre)
                .Take(5)
                .ToList();
            return Json(sugerencias);
        }

        private static int CategoriaClima(string clima)
        {
            var valor = clima.ToLowerInvariant();
            if (valor.Contains("cold")) return 0;
            if (valor.Contains("temperate")) return 1;
            if (valor.Contains("warm") || valor.Contains("hot")) return 2;
            return 1;
        }

        private static int NivelAltura(string altura)
        {
            var valor = altura.ToLowerInvariant();
            if (valor.Contains("low")) return 0;
            if (valor.Contains("medium")) return 1;
            if (valor.Contains("high")) return 2;
            if (valor.Contains("extreme")) return 3;
            return 1;
        }
    }
}
