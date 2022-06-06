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
    public class GarantiaController : Controller
    {
        private readonly easycreditContext _context;

        public GarantiaController(easycreditContext context)
        {
            _context = context;
        }

        // GET: Garantia
        public async Task<IActionResult> Index()
        {
            var easycreditContext = _context.Garantia.Include(g => g.Tipo);
            return View(await easycreditContext.ToListAsync());
        }

        // GET: Garantia/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Garantia == null)
            {
                return NotFound();
            }

            var garantium = await _context.Garantia
                .Include(g => g.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (garantium == null)
            {
                return NotFound();
            }

            return View(garantium);
        }

        // GET: Garantia/Create
        public IActionResult Create()
        {
            ViewData["tipos"] = _context.TipoGarantia.Where(x => x.Active == true).ToList();
            return View();
        }

        // POST: Garantia/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Codigo,Valor,Ubicacion,TipoId,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] Garantium garantium)
        {
            if (ModelState.IsValid)
            {
                _context.Add(garantium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TipoId"] = new SelectList(_context.TipoGarantia, "Id", "Id", garantium.TipoId);
            return View(garantium);
        }

        // GET: Garantia/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Garantia == null)
            {
                return NotFound();
            }

            var garantium = await _context.Garantia.FindAsync(id);
            if (garantium == null)
            {
                return NotFound();
            }
            ViewData["TipoId"] = new SelectList(_context.TipoGarantia, "Id", "Id", garantium.TipoId);
            return View(garantium);
        }

        // POST: Garantia/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Codigo,Valor,Ubicacion,TipoId,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] Garantium garantium)
        {
            if (id != garantium.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(garantium);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GarantiumExists(garantium.Id))
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
            ViewData["TipoId"] = new SelectList(_context.TipoGarantia, "Id", "Id", garantium.TipoId);
            return View(garantium);
        }

        // GET: Garantia/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Garantia == null)
            {
                return NotFound();
            }

            var garantium = await _context.Garantia
                .Include(g => g.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (garantium == null)
            {
                return NotFound();
            }

            return View(garantium);
        }

        // POST: Garantia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Garantia == null)
            {
                return Problem("Entity set 'easycreditContext.Garantia'  is null.");
            }
            var garantium = await _context.Garantia.FindAsync(id);
            if (garantium != null)
            {
                _context.Garantia.Remove(garantium);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GarantiumExists(int id)
        {
          return (_context.Garantia?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
