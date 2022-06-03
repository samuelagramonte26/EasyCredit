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
    public class TipoGarantiaController : Controller
    {
        private readonly easycreditContext _context;

        public TipoGarantiaController(easycreditContext context)
        {
            _context = context;
        }

        // GET: TipoGarantia
        public async Task<IActionResult> Index()
        {
              return _context.TipoGarantia != null ? 
                          View(await _context.TipoGarantia.ToListAsync()) :
                          Problem("Entity set 'easycreditContext.TipoGarantia'  is null.");
        }

        // GET: TipoGarantia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TipoGarantia == null)
            {
                return NotFound();
            }

            var tipoGarantium = await _context.TipoGarantia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoGarantium == null)
            {
                return NotFound();
            }

            return View(tipoGarantium);
        }

        // GET: TipoGarantia/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoGarantia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tipo,Descripcion,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] TipoGarantium tipoGarantium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoGarantium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoGarantium);
        }

        // GET: TipoGarantia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TipoGarantia == null)
            {
                return NotFound();
            }

            var tipoGarantium = await _context.TipoGarantia.FindAsync(id);
            if (tipoGarantium == null)
            {
                return NotFound();
            }
            return View(tipoGarantium);
        }

        // POST: TipoGarantia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tipo,Descripcion,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] TipoGarantium tipoGarantium)
        {
            if (id != tipoGarantium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoGarantium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoGarantiumExists(tipoGarantium.Id))
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
            return View(tipoGarantium);
        }

        // GET: TipoGarantia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoGarantia == null)
            {
                return NotFound();
            }

            var tipoGarantium = await _context.TipoGarantia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoGarantium == null)
            {
                return NotFound();
            }

            return View(tipoGarantium);
        }

        // POST: TipoGarantia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoGarantia == null)
            {
                return Problem("Entity set 'easycreditContext.TipoGarantia'  is null.");
            }
            var tipoGarantium = await _context.TipoGarantia.FindAsync(id);
            if (tipoGarantium != null)
            {
                _context.TipoGarantia.Remove(tipoGarantium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoGarantiumExists(int id)
        {
          return (_context.TipoGarantia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
