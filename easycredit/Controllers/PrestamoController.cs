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
    public class PrestamoController : Controller
    {
        private readonly easycreditContext _context;

        public PrestamoController(easycreditContext context)
        {
            _context = context;
        }

        // GET: Prestamo
        public async Task<IActionResult> Index()
        {
            var easycreditContext = _context.Prestamos.Where(x=>x.Active == true && x.Saldado == false).Include(p => p.Cliente).Include(p => p.Garante).Include(p => p.Garantia);
            return View(await easycreditContext.ToListAsync());
        }

        // GET: Prestamo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Cliente)
                .Include(p => p.Garante)
                .Include(p => p.Garantia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamo/Create
        public IActionResult Create()
        {
            ViewData["id"] = _context.Prestamos.Where(x=>x.Active==true).OrderByDescending(p => p.Id).FirstOrDefault()?.Id;

            ViewData["clientes"] = _context.Clientes.Where(x => x.Active == true && x.Tipo.Tipo == "prestario" ).ToList();
            ViewData["garantes"] = _context.Clientes.Where(x => x.Active == true && x.Tipo.Tipo=="garante").ToList();
            ViewData["garantias"] =_context.Garantia.Where(x => x.Active == true);
            return View();
        }

        // POST: Prestamo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Monto,Plazo,TazaInteres,FechaSolicitud,FechaAprovacion,FechaInicio,FechaTermino,ClienteId,GaranteId,GarantiaId,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active,Saldado")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                prestamo.TazaInteres = ((prestamo.TazaInteres/12) / 100);
                prestamo.Plazo = (prestamo.Plazo * 12);
                prestamo.FechaSolicitud = DateTime.Today.Date;

                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", prestamo.ClienteId);
            ViewData["GaranteId"] = new SelectList(_context.Clientes, "Id", "Id", prestamo.GaranteId);
            ViewData["GarantiaId"] = new SelectList(_context.Garantia, "Id", "Id", prestamo.GarantiaId);
            return View(prestamo);
        }

        // GET: Prestamo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            prestamo.TazaInteres = ((prestamo.TazaInteres * 12) * 100);
            prestamo.Plazo= (prestamo.Plazo / 12);
            ViewData["clientes"] = _context.Clientes.Where(x => x.Active == true && x.Tipo.Tipo == "prestario").ToList();
            ViewData["garantes"] = _context.Clientes.Where(x => x.Active == true && x.Tipo.Tipo == "garante").ToList();
            ViewData["garantias"] = _context.Garantia.Where(x => x.Active == true);
            return View(prestamo);
        }

        // POST: Prestamo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Monto,Plazo,TazaInteres,FechaSolicitud,FechaAprovacion,FechaInicio,FechaTermino,ClienteId,GaranteId,GarantiaId,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active,Saldado")] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    prestamo.FechaEditado = DateTime.Today.Date;
                    prestamo.TazaInteres = ((prestamo.TazaInteres / 12) / 100);
                    prestamo.Plazo = (prestamo.Plazo * 12);
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", prestamo.ClienteId);
            ViewData["GaranteId"] = new SelectList(_context.Clientes, "Id", "Id", prestamo.GaranteId);
            ViewData["GarantiaId"] = new SelectList(_context.Garantia, "Id", "Id", prestamo.GarantiaId);
            return View(prestamo);
        }

        // GET: Prestamo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prestamos == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Cliente)
                .Include(p => p.Garante)
                .Include(p => p.Garantia)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prestamos == null)
            {
                return Problem("Entity set 'easycreditContext.Prestamos'  is null.");
            }
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                prestamo.Active = false;
                var pagos = _context.Pagos.Where(x => x.CodigoPrestamo == prestamo.Id && x.Active == true);
                foreach (var pago in pagos)
                {
                    pago.Active = false;
                    pago.FechaEliminado = DateTime.Today.Date;
                     _context.Update(pago);
                    // _context.SaveChanges();

                }
                prestamo.Active = false;
                prestamo.FechaEliminado = DateTime.Today.Date;
                _context.Update(prestamo);
            }

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["error"] = "No se puede eliminar este prestamo primero debe eliminar los pagos relacionados al mismo!";
                return View(prestamo);

            }
        }

        private bool PrestamoExists(int id)
        {
          return (_context.Prestamos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
