namespace easycredit.Models
{
    public class TopAtrazo
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public double Monto { get; set; }
        public double MontoPendiente { get; set; }
        public double DiasAtrazo { get; set; }
        public int CuotasPagadas { get; set; }
        public int Inicio { get; set; }
        public int Fin { get; set; }
        public int CuotasPendientes { get; set; }
        public DateTime FechaPlanificada { get; set; }
        public DateTime FechaUltimoPago { get; set; }

        public bool HasPagos { get; set; }
        
    }
}
