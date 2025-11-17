namespace MVP_ProyectoFinal.Models
{
    public class ResultadoIntentoBiomaVM
    {
        public string NombreBioma { get; set; } = string.Empty;
        public string Clima { get; set; } = string.Empty;
        public string Precipitacion { get; set; } = string.Empty;
        public string Dimension { get; set; } = string.Empty;
        public string Altura { get; set; } = string.Empty;
        public int YearLanzamiento { get; set; }
        public string VersionLanzamiento { get; set; } = string.Empty;

        public int LongitudNombre { get; set; }
        public string CoincideInicial { get; set; } = string.Empty;
        public string ColorLongitudNombre { get; set; } = string.Empty;
        public string ColorCoincideInicial { get; set; } = string.Empty;
        public string HintLongitudNombre { get; set; } = string.Empty;

        public string ColorClima { get; set; } = string.Empty;
        public string ColorPrecipitacion { get; set; } = string.Empty;
        public string ColorDimension { get; set; } = string.Empty;
        public string ColorAltura { get; set; } = string.Empty;
        public string ColorAnio { get; set; } = string.Empty;

        public string HintClima { get; set; } = string.Empty;
        public string HintPrecipitacion { get; set; } = string.Empty;
        public string HintAltura { get; set; } = string.Empty;
        public string HintAnio { get; set; } = string.Empty;
    }
}
