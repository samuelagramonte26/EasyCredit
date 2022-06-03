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
            var easycreditContext = _context.Inversions.Include(i => i.Cliente);
            return View(await easycreditContext.ToListAsync());
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id");
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", inversion.ClienteId);
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
                _context.Inversions.Remove(inversion);
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