namespace MauiAppTempoAgora.Models
{
    public class Tempo
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string? Main { get; set; }
        public string? Description { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public double WindSpeed { get; set; }
        public int Visibility { get; set; }
        public string? Sunrise { get; set; }
        public string? Sunset { get; set; }
    }
}
