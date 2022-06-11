using easycredit.Data;
using easycredit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace easycredit.Controllers
{
    public class ReporteController : Controller
    {
        private ILogger<ReporteController> _logger { get; }

        public ReporteController(easycreditContext context, ILogger<ReporteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public easycreditContext _context { get; }

        public async Task<IActionResult> Amortizacion(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            double monto = (double)prestamo.Monto;
            double taza = (double)prestamo.TazaInteres;
            double plazo = (double)prestamo.Plazo;
            double cuotaFijaMensual = (double)(monto * ((Math.Pow((1 + taza), plazo) * taza) / (Math.Pow((1 + taza), plazo) - 1)));
            var amortizacion = new List<Amortizacion>();

            for (int i = 1; i <= plazo; i++)
            {
                var fecha = prestamo.FechaInicio.Value;
                var interes = monto * taza;
                var capital = Math.Round((cuotaFijaMensual - interes), 2);
                var saldo = Math.Round((monto - capital), 2);
                amortizacion.Add(new Amortizacion()
                {
                    Numero = i,
                    CapitalInicial = monto,
                    Cuota = cuotaFijaMensual,
                    Interes = interes,
                    Abono = capital,
                    SaldoPendiente = Math.Abs(saldo),
                    FechaPlanificada = fecha.AddMonths(i).ToString("dd - MMM - yyyy")
                });
                monto = saldo;

            }

            return View("Amortizacion", amortizacion.ToList());

        }
        public async  Task<IActionResult> CuotaPrestamo(int id)
        {

            var cuotas = _context.Pagos.Where(x => x.CodigoPrestamo == id && x.Active == true && x.CodigoPrestamoNavigation.Saldado == false).Include(x => x.CodigoPrestamoNavigation).Include(x => x.CodigoPrestamoNavigation.Cliente);

            return View("CuotaPrestamo",cuotas);
        }
   
        public async Task<IActionResult> Top(int inicio, int fin)
        {
           List<TopAtrazo> top = new List<TopAtrazo>();
        
            var prestamos = _context.Prestamos.Where(x => x.Active == true && x.Saldado == false).Include( p=>p.Cliente);
            foreach (var p in prestamos)
            {
                var fechaInicio = p.FechaInicio.Value;

                var fechaA = DateTime.Today.Date;
                int total = DateTime.DaysInMonth(fechaA.Year, fechaA.Month);
                var fechaI = fechaInicio;

                var diaPago = fechaI.Day;
                var fechaPlanificada = fechaI.AddDays(((fechaA - fechaI).TotalDays));

                if (total == 30 && diaPago == 31)
                {
                    diaPago = 1;
                }
              //  fechaPlanificada = Convert.ToDateTime($"{diaPago}/{fechaPlanificada.Month}/{fechaPlanificada.Year}");
               // fechaPlanificada = fechaPlanificada.AddMonths(1);
                var pago = await _context.Pagos
                    .Where(x => x.Active == true && x.CodigoPrestamo == p.Id)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                var pagos = _context.Pagos
                    .Where(x => x.Active == true && x.CodigoPrestamo == p.Id);
              
                fechaPlanificada = fechaI.AddMonths((pagos.Count()));

                var fechaU = (pago != null ) ? pago.FechaEfectiva.Value: fechaA;
                var dif = (fechaU - fechaPlanificada).TotalDays;
                _logger.LogWarning($"dif: {dif} fechaUlt:{fechaU} fechapla:{fechaPlanificada} id:{p.Id}" );
                if(dif >= inicio && dif <= fin)
                {
                    //_logger.LogWarning($"Se encontro el malapaga ID:{p.Id} " +
                    //    $"atraso de {dif} dias " +
                    //    $"fechaultimo:{fechaU} " +
                    //    $"fechaPla:{fechaPlanificada}");
                    top.Add(new TopAtrazo
                    {
                        Codigo = p.Codigo,
                        Nombre = p.Cliente.Nombre,
                        Apellido = p.Cliente.Apellido,
                        Monto = (double)p.Monto,
                        FechaPlanificada = fechaPlanificada,
                        FechaUltimoPago = fechaU,
                        HasPagos = (pago != null) ? true : false,
                        CuotasPagadas = pagos.Count(),
                        CuotasPendientes =(int)(p.Plazo - pagos.Count()),
                        DiasAtrazo = dif,
                        MontoPendiente =(pago != null) ? (double)pago.Amortizacion : (double)p.Monto,
                        Inicio = inicio,
                        Fin = fin

                    });
                    
                }
            }
            return View("Top",top);
        }
    }
}
