namespace easycredit.Models
{
    public class Amortizacion
    {
        public int Numero { get; set; }
        public double CapitalInicial { get; set; }
        public double Cuota { get; set; }
        public double Interes { get; set; }
        public double Abono { get; set; }
        public double SaldoPendiente { get; set; }
        public string? FechaPlanificada { get; set; }
    }
}
