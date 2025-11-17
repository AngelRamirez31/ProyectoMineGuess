using MVP_ProyectoFinal.Models.Elementos;

namespace MVP_ProyectoFinal.Models
{
    public static class RepositorioBiomas
    {
        private static bool _useApi = false;

        private static string? _apiBaseUrl = null;

        private static List<Bioma>? _cacheApi = null;

        public static void ConfigurarApi(bool useApi, string? baseUrl) { _useApi = useApi; _apiBaseUrl = baseUrl; _cacheApi = null; }

        private static readonly Random _random = new Random();

        private static readonly List<Bioma> _biomas = new List<Bioma>
        {
            new Bioma { Id = 1, Nombre = "Plains", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 2, Nombre = "Sunflower Plains", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 3, Nombre = "Forest", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 4, Nombre = "Flower Forest", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 5, Nombre = "Birch Forest", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 6, Nombre = "Dark Forest", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2014, VersionLanzamiento = "1.7" },
            new Bioma { Id = 7, Nombre = "Jungle", Clima = "Warm", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2012, VersionLanzamiento = "1.2.1" },
            new Bioma { Id = 8, Nombre = "Bamboo Jungle", Clima = "Warm", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2019, VersionLanzamiento = "1.0" },
            new Bioma { Id = 9, Nombre = "Savanna", Clima = "Warm", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 10, Nombre = "Desert", Clima = "Warm", Precipitacion = "None", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 11, Nombre = "Badlands", Clima = "Hot", Precipitacion = "None", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 12, Nombre = "Wooded Badlands", Clima = "Hot", Precipitacion = "None", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 13, Nombre = "Eroded Badlands", Clima = "Hot", Precipitacion = "None", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 14, Nombre = "Swamp", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 15, Nombre = "Mangrove Swamp", Clima = "Warm", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2022, VersionLanzamiento = "1.19" },
            new Bioma { Id = 16, Nombre = "Taiga", Clima = "Cold", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 17, Nombre = "Snowy Taiga", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 18, Nombre = "Mountains", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 19, Nombre = "Jagged Peaks", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "Extreme", YearLanzamiento = 2021, VersionLanzamiento = "1.18" },
            new Bioma { Id = 20, Nombre = "Frozen Peaks", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2021, VersionLanzamiento = "1.18" },
            new Bioma { Id = 21, Nombre = "Stony Peaks", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2021, VersionLanzamiento = "1.18" },
            new Bioma { Id = 22, Nombre = "Meadow", Clima = "Cold", Precipitacion = "Rain", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2021, VersionLanzamiento = "1.18" },
            new Bioma { Id = 23, Nombre = "Cherry Grove", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2023, VersionLanzamiento = "1.20" },
            new Bioma { Id = 24, Nombre = "Lush Caves", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2021, VersionLanzamiento = "1.18" },
            new Bioma { Id = 25, Nombre = "Dripstone Caves", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2021, VersionLanzamiento = "1.18" },
            new Bioma { Id = 26, Nombre = "Ocean", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 27, Nombre = "Frozen Ocean", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2018, VersionLanzamiento = "1.13" },
            new Bioma { Id = 28, Nombre = "Soul Sand Valley", Clima = "Hot", Precipitacion = "None", Dimension = "Nether", Altura = "Medium", YearLanzamiento = 2020, VersionLanzamiento = "1.16" },
            new Bioma { Id = 29, Nombre = "Basalt Deltas", Clima = "Hot", Precipitacion = "None", Dimension = "Nether", Altura = "High", YearLanzamiento = 2020, VersionLanzamiento = "1.16" },
            new Bioma { Id = 30, Nombre = "Crimson Forest", Clima = "Hot", Precipitacion = "None", Dimension = "Nether", Altura = "Medium", YearLanzamiento = 2020, VersionLanzamiento = "1.16" },
            new Bioma { Id = 31, Nombre = "Warped Forest", Clima = "Hot", Precipitacion = "None", Dimension = "Nether", Altura = "Medium", YearLanzamiento = 2020, VersionLanzamiento = "1.16" },
            new Bioma { Id = 32, Nombre = "End Highlands", Clima = "Cold", Precipitacion = "None", Dimension = "End", Altura = "High", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 33, Nombre = "Deep Dark", Clima = "Cold", Precipitacion = "None", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2022, VersionLanzamiento = "1.19" },
            new Bioma { Id = 34, Nombre = "Mushroom Fields", Clima = "Temperate", Precipitacion = "None", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 35, Nombre = "Snowy Plains", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 36, Nombre = "Ice Spikes", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 37, Nombre = "Windswept Hills", Clima = "Cold", Precipitacion = "Rain", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 38, Nombre = "Windswept Forest", Clima = "Cold", Precipitacion = "Rain", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 39, Nombre = "Windswept Gravelly Hills", Clima = "Cold", Precipitacion = "Rain", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 40, Nombre = "Windswept Savanna", Clima = "Warm", Precipitacion = "Rain", Dimension = "Overworld", Altura = "High", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 41, Nombre = "Old Growth Pine Taiga", Clima = "Cold", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 42, Nombre = "Old Growth Spruce Taiga", Clima = "Cold", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 43, Nombre = "Old Growth Birch Forest", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 44, Nombre = "Sparse Jungle", Clima = "Warm", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Medium", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 45, Nombre = "Stony Shore", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 46, Nombre = "Beach", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 47, Nombre = "Snowy Beach", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 48, Nombre = "River", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 49, Nombre = "Frozen River", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 50, Nombre = "Warm Ocean", Clima = "Warm", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2018, VersionLanzamiento = "1.13" },
            new Bioma { Id = 51, Nombre = "Lukewarm Ocean", Clima = "Warm", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2018, VersionLanzamiento = "1.13" },
            new Bioma { Id = 52, Nombre = "Cold Ocean", Clima = "Cold", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2018, VersionLanzamiento = "1.13" },
            new Bioma { Id = 53, Nombre = "Deep Ocean", Clima = "Temperate", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2013, VersionLanzamiento = "1.7" },
            new Bioma { Id = 54, Nombre = "Deep Lukewarm Ocean", Clima = "Warm", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2018, VersionLanzamiento = "1.13" },
            new Bioma { Id = 55, Nombre = "Deep Cold Ocean", Clima = "Cold", Precipitacion = "Rain", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2018, VersionLanzamiento = "1.13" },
            new Bioma { Id = 56, Nombre = "Deep Frozen Ocean", Clima = "Cold", Precipitacion = "Snow", Dimension = "Overworld", Altura = "Low", YearLanzamiento = 2018, VersionLanzamiento = "1.13" },
            new Bioma { Id = 57, Nombre = "Nether Wastes", Clima = "Hot", Precipitacion = "None", Dimension = "Nether", Altura = "Medium", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 58, Nombre = "Small End Islands", Clima = "Cold", Precipitacion = "None", Dimension = "End", Altura = "High", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 59, Nombre = "End Midlands", Clima = "Cold", Precipitacion = "None", Dimension = "End", Altura = "High", YearLanzamiento = 2011, VersionLanzamiento = "1.0" },
            new Bioma { Id = 60, Nombre = "End Barrens", Clima = "Cold", Precipitacion = "None", Dimension = "End", Altura = "High", YearLanzamiento = 2011, VersionLanzamiento = "1.0" }
        };

        public static List<Bioma> ObtenerTodos()
        {
            if (_useApi && !string.IsNullOrWhiteSpace(_apiBaseUrl))
            {
                try
                {
                    if (_cacheApi is null)
                    {
                        var cli = new Services.ApiClientOriginal(_apiBaseUrl!);
                        _cacheApi = cli.GetBiomesAsync().GetAwaiter().GetResult();
                    }
                    if (_cacheApi is not null && _cacheApi.Count > 0)
                    {
                        return _cacheApi;
                    }
                }
                catch
                {
                    _useApi = false;
                    _cacheApi = new List<Bioma>();
                }
            }
            return _biomas;
        }

        public static Bioma? ObtenerPorNombre(string nombre)
        {
            var lista = ObtenerTodos();
            return lista.FirstOrDefault(b => b.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

        public static Bioma? ObtenerBiomaAleatorio()
        {
            var lista = ObtenerTodos();
            if (lista.Count == 0) return null;
            var index = _random.Next(lista.Count);
            return lista[index];
        }
    }
}
