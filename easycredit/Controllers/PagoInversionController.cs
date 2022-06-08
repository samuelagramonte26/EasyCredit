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
    public class PagoInversionController : Controller
    {
        private readonly easycreditContext _context;

        public PagoInversionController(easycreditContext context)
        {
            _context = context;
        }

        // GET: PagoInversion
        public async Task<IActionResult> Index()
        {
            var easycreditContext = _context.PagoInversions.Include(p => p.CodigoInversionNavigation).Include(p => p.ModalidadNavigation).Where(x=>x.Active == true);
            return View(await easycreditContext.ToListAsync());
        }

        // GET: PagoInversion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PagoInversions == null)
            {
                return NotFound();
            }

            var pagoInversion = await _context.PagoInversions
                .Include(p => p.CodigoInversionNavigation)
                .Include(p => p.CodigoInversionNavigation.Cliente)
                .Include(p => p.ModalidadNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pagoInversion == null)
            {
                return NotFound();
            }

            return View(pagoInversion);
        }

        // GET: PagoInversion/Create
        public IActionResult Create()
        {

            ViewData["CodigoInversion"] = _context.Inversions.Where(x => x.Active == true && x.Saldado == false).ToList();
            ViewData["Modalidad"] = _context.ModalidadPagos.Where(x => x.Active == true).ToList();
            ViewData["id"] = _context.PagoInversions.Where(x => x.Active == true).OrderByDescending(p => p.Id).FirstOrDefault()?.Id;
            return View();
        }

        // POST: PagoInversion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CapitalInicial,FechaPlanificada,FechaEfectiva,Cuota,Interes,Abono,Amortizacion,Modalidad,CodigoInversion,CodigoComprobante,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] PagoInversion pagoInversion)
        {

                var codigo = pagoInversion.CodigoInversion;
            var inversion = _context.Inversions.Where(x=>x.Active==true).FirstOrDefault(p => p.Id == codigo);

            var taza = inversion?.TazaInteres;
            var monto = inversion?.Monto;
             var pagos = _context.PagoInversions.Where(x => x.CodigoInversion == codigo && x.Active == true).OrderByDescending(x=>x.Id).FirstOrDefault();
            double? amort = 0;
            double? capital = 0;
            PagoInversion pago1 = new PagoInversion();
            var numPagos = _context.PagoInversions.Where(x => x.CodigoInversion == codigo && x.Active == true).ToList().Count();

            if (pagos != null)
            {
                pago1.FechaPlanificada = inversion.FechaInicio.Value.AddMonths(numPagos);
                if(pagos.Amortizacion < 1)
                {
                    inversion.Saldado = true;
                    _context.Update(inversion);
                    await _context.SaveChangesAsync();
                   return RedirectToAction(nameof(Index))
;                }
              amort = pagos.Amortizacion;
                capital = amort;
            }
            else
            {
                pago1.FechaPlanificada = inversion.FechaInicio.Value;
                amort = monto;
                capital = monto;
            }
            
            var interes = amort * taza;
            var cuota = pagoInversion.Cuota;
            double? abono = cuota- interes;

            pago1.Cuota = cuota;
            pago1.CodigoInversion = codigo;
            pago1.Abono =abono;
            pago1.CodigoComprobante = pagoInversion.CodigoComprobante;
            pago1.Amortizacion = amort - abono;
            pago1.CapitalInicial = capital;
            pago1.Modalidad = pagoInversion.Modalidad;
            pago1.FechaEfectiva = DateTime.Today.Date;
            pago1.FechaCreado = DateTime.Today.Date;
            pago1.Interes = interes;
            _context.Add(pago1);
           if(pago1.Amortizacion < 1)
            {
                inversion.Saldado = true;
                _context.Update(inversion);
                await _context.SaveChangesAsync();
         
            }
            await _context.SaveChangesAsync();

            
            return RedirectToAction(nameof(Index));
            //if (ModelState.IsValid)
            //{
            //    _context.Add(pagoInversion);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CodigoInversion"] = new SelectList(_context.Inversions, "Id", "Id", pagoInversion.CodigoInversion);
            //ViewData["Modalidad"] = new SelectList(_context.ModalidadPagos, "Id", "Id", pagoInversion.Modalidad);
            //return View(pagoInversion);
        }

        // GET: PagoInversion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PagoInversions == null)
            {
                return NotFound();
            }

            var pagoInversion = await _context.PagoInversions.FindAsync(id);
            if (pagoInversion == null)
            {
                return NotFound();
            }
            ViewData["CodigoInversion"] = new SelectList(_context.Inversions, "Id", "Id", pagoInversion.CodigoInversion);
            ViewData["Modalidad"] = new SelectList(_context.ModalidadPagos, "Id", "Id", pagoInversion.Modalidad);
            return View(pagoInversion);
        }

        // POST: PagoInversion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CapitalInicial,FechaPlanificada,FechaEfectiva,Cuota,Interes,Abono,Amortizacion,Modalidad,CodigoInversion,CodigoComprobante,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] PagoInversion pagoInversion)
        {
            if (id != pagoInversion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pagoInversion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoInversionExists(pagoInversion.Id))
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
            ViewData["CodigoInversion"] = new SelectList(_context.Inversions, "Id", "Id", pagoInversion.CodigoInversion);
            ViewData["Modalidad"] = new SelectList(_context.ModalidadPagos, "Id", "Id", pagoInversion.Modalidad);
            return View(pagoInversion);
        }

        // GET: PagoInversion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PagoInversions == null)
            {
                return NotFound();
            }

            var pagoInversion = await _context.PagoInversions
                .Include(p => p.CodigoInversionNavigation)
                .Include(p => p.ModalidadNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pagoInversion == null)
            {
                return NotFound();
            }

            return View(pagoInversion);
        }

        // POST: PagoInversion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PagoInversions == null)
            {
                return Problem("Entity set 'easycreditContext.PagoInversions'  is null.");
            }
            var pagoInversion = await _context.PagoInversions.FindAsync(id);
            if (pagoInversion != null)
            {
                _context.PagoInversions.Remove(pagoInversion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoInversionExists(int id)
        {
          return (_context.PagoInversions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
