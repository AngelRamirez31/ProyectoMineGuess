namespace MVP_ProyectoFinal.Models.Elementos
{
    public class Bioma
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Clima { get; set; } = string.Empty;
        public string Precipitacion { get; set; } = string.Empty;
        public string Dimension { get; set; } = string.Empty;
        public string Altura { get; set; } = string.Empty;
        public int YearLanzamiento { get; set; }
        public string VersionLanzamiento { get; set; } = string.Empty;
    }
}
