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
    public class PagoController : Controller
    {
        private readonly easycreditContext _context;

        public PagoController(easycreditContext context)
        {
            _context = context;

        }

        // GET: Pago
        public async Task<IActionResult> Index()
        {
            var easycreditContext = _context.Pagos.Include(p => p.CodigoPrestamoNavigation).Include(p => p.ModalidadNavigation).Where(x=> x.Active == true);
            return View(await easycreditContext.ToListAsync());
        }

        // GET: Pago/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pagos == null)
            {
                return NotFound();
            }
           
            var pago = await _context.Pagos
                .Include(p => p.CodigoPrestamoNavigation)
                .Include(p => p.ModalidadNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pago/Create
        public IActionResult Create()
        {
            ViewData["CodigoPrestamo"] =_context.Prestamos.Where(x => x.Active==true).ToList();
            ViewData["Modalidad"] = _context.ModalidadPagos.Where(x => x.Active == true).ToList();
            ViewData["id"] = _context.Pagos.Where(x=>x.Active==true).OrderByDescending(p => p.Id).FirstOrDefault()?.Id;
            return View();
        }

        // POST: Pago/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CapitalInicial,FechaPlanificada,FechaEfectiva,Cuota,Interes,Amortizacion,Modalidad,CodigoComprobante,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active,Abono,CodigoPrestamo")] Pago pago)
        {
            var codigo = pago.CodigoPrestamo;
            var prestamo = _context.Prestamos.Where(x=>x.Active==true).FirstOrDefault(p => p.Id == codigo);

            var taza = prestamo?.TazaInteres;
            var monto = prestamo?.Monto;
            //var interes = prestamo?.TazaInteres;
          //  var plazo = prestamo?.Plazo;
          //  var cuotaFijaMensual = (float?)(monto * ((Math.Pow((1 + (double)taza), (double)plazo) * taza) / (Math.Pow((1 + (double)taza), (double)plazo) - 1)));

            // var cuotaFija = monto * ((Math.Pow((double)(1 + interes), (double)plazo) * interes) / (Math.Pow((double)(1 + interes), (double)plazo) - 1));
            var pagos = _context.Pagos.Where(x => x.CodigoPrestamo == codigo && x.Active == true).OrderByDescending(x=>x.Id).FirstOrDefault();
            double? amort = 0;
            double? capital = 0;

            if (pagos != null)
            {
              amort = pagos.Amortizacion;
                capital = amort;
            }
            else
            {
                amort = monto;
                capital = monto;
            }
            Pago pago1 = new Pago();
            var interes = amort * taza;
            var cuota = pago.Cuota;
            double? abono = cuota- interes;

            pago1.Cuota = cuota;
            pago1.CodigoPrestamo = codigo;
            pago1.Abono =abono;
            pago1.CodigoComprobante = pago.CodigoComprobante;
            pago1.Amortizacion = amort - abono;
            pago1.CapitalInicial = capital;
            pago1.Modalidad = pago.Modalidad;
            pago1.FechaEfectiva = DateTime.Today.Date;
            pago1.FechaCreado = DateTime.Today.Date;
            pago1.Interes = interes;
            _context.Add(pago1);
           
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            //if (ModelState.IsValid)
            //{
            //    _context.Add(pago);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["CodigoPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id", pago.CodigoPrestamo);
            //ViewData["Modalidad"] = new SelectList(_context.ModalidadPagos, "Id", "Id", pago.Modalidad);
            //return View(pago);
        }

        // GET: Pago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pagos == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
            {
                return NotFound();
            }
            ViewData["CodigoPrestamo"] = _context.Prestamos.ToList();
            ViewData["Modalidad"] = _context.ModalidadPagos.ToList();
            ViewData["id"] = _context.Pagos.Where(x => x.Active == true).OrderByDescending(p => p.Id).FirstOrDefault().Id;
            return View(pago);
        }

        // POST: Pago/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CapitalInicial,FechaPlanificada,FechaEfectiva,Cuota,Interes,Amortizacion,Modalidad,CodigoComprobante,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active,Abono,CodigoPrestamo")] Pago pago)
        {
          
            if (id != pago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                 //  var pago1 = await _context.Pagos.FindAsync(id);
                    var codigo = pago.CodigoPrestamo;
                    var prestamo = _context.Prestamos.Where(x=>x.Active==true).FirstOrDefault(p => p.Id == codigo);

                    var taza = prestamo?.TazaInteres;
                    var monto = prestamo?.Monto;
                   // var pagos = _context.Pagos.Where(x => x.CodigoPrestamo == codigo).OrderByDescending(x => x.Id).FirstOrDefault();
                    double? amort = 0;
                    double? capital = 0;
                  //  if (pagos != null)
                  //  {
                      //  amort = pago.Amortizacion;
                        capital = pago.CapitalInicial;
                   // }
                   // else
                  //  {
                    //    amort = monto;
                  //      capital = monto;
                  //  }
                  //  Pago pago1 = new Pago();

                    var interes = capital * taza;
                    var cuota = pago.Cuota;
                    double? abono = cuota - interes;
                    //pago1.Id = pago.Id;
                 //    pago.Cuota = cuota;
                    pago.CodigoPrestamo = codigo;
                    pago.Abono = abono;
                  //  pago1.CodigoComprobante = pago.CodigoComprobante;
                    pago.Amortizacion = capital - abono;
                    pago.CapitalInicial = capital;
                    pago.Modalidad = pago.Modalidad;
                    
                    pago.FechaEditado = DateTime.Today.Date;
                    pago.Interes = interes;

                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.Id))
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
            ViewData["CodigoPrestamo"] = new SelectList(_context.Prestamos, "Id", "Id", pago.CodigoPrestamo);
            ViewData["Modalidad"] = new SelectList(_context.ModalidadPagos, "Id", "Id", pago.Modalidad);
            return View(pago);
        }

        // GET: Pago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pagos == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.CodigoPrestamoNavigation)
                .Include(p => p.ModalidadNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pagos == null)
            {
                return Problem("Entity set 'easycreditContext.Pagos'  is null.");
            }
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
            {
                pago.Active = false;
                pago.FechaEliminado = DateTime.Today.Date;
                _context.Update(pago);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
          return (_context.Pagos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
