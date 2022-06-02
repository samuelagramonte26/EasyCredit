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
    public class ModalidadPagoController : Controller
    {
        private readonly easycreditContext _context;

        public ModalidadPagoController(easycreditContext context)
        {
            _context = context;
        }

        // GET: ModalidadPago
        public async Task<IActionResult> Index()
        {
              return _context.ModalidadPagos != null ? 
                          View(await _context.ModalidadPagos.ToListAsync()) :
                          Problem("Entity set 'easycreditContext.ModalidadPagos'  is null.");
        }

        // GET: ModalidadPago/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ModalidadPagos == null)
            {
                return NotFound();
            }

            var modalidadPago = await _context.ModalidadPagos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modalidadPago == null)
            {
                return NotFound();
            }

            return View(modalidadPago);
        }

        // GET: ModalidadPago/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ModalidadPago/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tipo,Descripcion,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] ModalidadPago modalidadPago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modalidadPago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modalidadPago);
        }

        // GET: ModalidadPago/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ModalidadPagos == null)
            {
                return NotFound();
            }

            var modalidadPago = await _context.ModalidadPagos.FindAsync(id);
            if (modalidadPago == null)
            {
                return NotFound();
            }
            return View(modalidadPago);
        }

        // POST: ModalidadPago/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tipo,Descripcion,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] ModalidadPago modalidadPago)
        {
            if (id != modalidadPago.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modalidadPago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModalidadPagoExists(modalidadPago.Id))
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
            return View(modalidadPago);
        }

        // GET: ModalidadPago/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ModalidadPagos == null)
            {
                return NotFound();
            }

            var modalidadPago = await _context.ModalidadPagos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modalidadPago == null)
            {
                return NotFound();
            }

            return View(modalidadPago);
        }

        // POST: ModalidadPago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ModalidadPagos == null)
            {
                return Problem("Entity set 'easycreditContext.ModalidadPagos'  is null.");
            }
            var modalidadPago = await _context.ModalidadPagos.FindAsync(id);
            if (modalidadPago != null)
            {
                _context.ModalidadPagos.Remove(modalidadPago);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModalidadPagoExists(int id)
        {
          return (_context.ModalidadPagos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
