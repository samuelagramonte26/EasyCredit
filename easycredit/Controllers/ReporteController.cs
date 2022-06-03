using easycredit.Data;
using easycredit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace easycredit.Controllers
{
    public class ReporteController : Controller
    {
        public ReporteController(Data.easycreditContext context)
        {
            _context = context;
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
    }
}
