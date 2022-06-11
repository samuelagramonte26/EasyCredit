using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using easycredit.Data;
using easycredit.Models;

namespace easycredit.Controllers
{
    public class InversionController : Controller
    {
        private readonly easycreditContext _context;

        public InversionController(easycreditContext context)
        {
            _context = context;
        }

        // GET: Inversion
        public async Task<IActionResult> Index()
        {
            var easycreditContext = _context.Inversions.Where(x => x.Active == true && x.Saldado == false).Include(i => i.Cliente);
            return View(await easycreditContext.ToListAsync());
        }
        public async Task<IActionResult> Vigentes()
        {
            var easycreditContext = _context.Inversions.Where(x => x.Active == true && x.Saldado == false).Include(p => p.Cliente);
            return View("InversionVigente", await easycreditContext.ToListAsync());
        }
        public async Task<IActionResult> Saldadas()
        {
            var inversiones = await _context.Inversions.Where(x => x.Active == true && x.Saldado == true).Include(x => x.Cliente).ToListAsync();


            return View("InversionSaldadas",inversiones);
        }

        public async Task<IActionResult> CuotaInversion(int id)
        {
            ViewData["id"] = id;
            var cuotas = _context.PagoInversions.Where(x => x.CodigoInversion == id && x.Active == true && x.CodigoInversionNavigation.Saldado == false).Include(x => x.CodigoInversionNavigation).Include(x => x.CodigoInversionNavigation.Cliente);

            return View("CuotaInversion",await cuotas.ToListAsync());
        }
        public async Task<IActionResult> Amotizacion(int id)
        {
            var prestamo = await _context.Inversions.FindAsync(id);
            double monto = (double)prestamo.Monto;
            double taza = (double)prestamo.TazaInteres;
            double plazo = (double)prestamo.Plazo;
            double cuotaFijaMensual = (double)(monto * ((Math.Pow((1 + taza), plazo) * taza) / (Math.Pow((1 + taza), plazo) - 1)));
            var amortizacion = new List<Amortizacion>();
            var fechaInicio = prestamo.FechaInicio.Value;

            for (int i = 1; i <= plazo; i++)
            {
                var interes = monto * taza;
                var capital = Math.Round((cuotaFijaMensual - interes), 2);
                var saldo = Math.Round((monto - capital), 2);
                fechaInicio = fechaInicio.AddMonths(1);
                amortizacion.Add(new Amortizacion()
                {
                    Numero = i,
                    CapitalInicial = monto,
                    Cuota = cuotaFijaMensual,
                    Interes = interes,
                    Abono = capital,
                    SaldoPendiente = Math.Abs(saldo),
                    FechaPlanificada = fechaInicio.ToString("dd - MMM - yyyy")
                });
                monto = saldo;

            }
            ViewData["id"] = id;
            return View("TablaAmortizacion", amortizacion.ToList());
        }
        // GET: Inversion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inversions == null)
            {
                return NotFound();
            }

            var inversion = await _context.Inversions
                .Include(i => i.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inversion == null)
            {
                return NotFound();
            }

            return View(inversion);
        }

        // GET: Inversion/Create
        public IActionResult Create()
        {
            ViewData["clientes"] =_context.Clientes.Where(x => x.Active == true).ToList();
            var listdo = _context.Inversions.Where(x => x.Active == true).OrderByDescending( i => i.Id).FirstOrDefault();
            ViewData["id"] = listdo?.Id;
            
            return View();
        }

        // POST: Inversion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Monto,TazaInteres,Plazo,FechaInicio,FechaTermino,ClienteId,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] Inversion inversion)
        {
            if (ModelState.IsValid)
            {
                inversion.FechaCreado = DateTime.Today.Date;
                var plazo = inversion.Plazo * 12;
                var interes = (inversion.TazaInteres / 12)/100;
                inversion.TazaInteres = interes;
                inversion.Plazo = plazo;
                inversion.Saldado = false;
                inversion.FechaTermino = inversion.FechaInicio.Value.AddMonths((int)plazo);
                _context.Add(inversion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", inversion.ClienteId);
            return View(inversion);
        }

        // GET: Inversion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inversions == null)
            {
                return NotFound();
            }

            var inversion = await _context.Inversions.FindAsync(id);
            if (inversion == null)
            {
                return NotFound();
            }
            inversion.Plazo = inversion.Plazo / 12;
            inversion.TazaInteres = (inversion.TazaInteres * 12)*100;
            ViewData["clientes"] = _context.Clientes.Where(x => x.Active == true).ToList();
            return View(inversion);
        }

        // POST: Inversion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Monto,TazaInteres,Plazo,FechaInicio,FechaTermino,ClienteId,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] Inversion inversion)
        {
            if (id != inversion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    inversion.Plazo = inversion.Plazo * 12;
                    inversion.TazaInteres = (inversion.TazaInteres /12)/ 100;
                    inversion.FechaEditado = DateTime.Today.Date;
                    _context.Update(inversion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InversionExists(inversion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", inversion.ClienteId);
            return View(inversion);
        }

        // GET: Inversion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inversions == null)
            {
                return NotFound();
            }

            var inversion = await _context.Inversions
                .Include(i => i.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inversion == null)
            {
                return NotFound();
            }

            return View(inversion);
        }

        // POST: Inversion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inversions == null)
            {
                return Problem("Entity set 'easycreditContext.Inversions'  is null.");
            }
            var inversion = await _context.Inversions.FindAsync(id);
            if (inversion != null)
            {
                inversion.FechaEliminado = DateTime.Today.Date;
                inversion.Active = false;

                _context.Update(inversion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InversionExists(int id)
        {
          return (_context.Inversions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
