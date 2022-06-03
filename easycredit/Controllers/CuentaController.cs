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
    public class CuentaController : Controller
    {
        private readonly easycreditContext _context;

        public CuentaController(easycreditContext context)
        {
            _context = context;
        }

        // GET: Cuenta
        public async Task<IActionResult> Index()
        {
            var easycreditContext = _context.Cuenta.Include(c => c.Cliente).Include(c => c.Tipo);
            return View(await easycreditContext.ToListAsync());
        }

        // GET: Cuenta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cuenta == null)
            {
                return NotFound();
            }

            var cuentum = await _context.Cuenta
                .Include(c => c.Cliente)
                .Include(c => c.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuentum == null)
            {
                return NotFound();
            }

            return View(cuentum);
        }

        // GET: Cuenta/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id");
            ViewData["TipoId"] = new SelectList(_context.TipoCuenta, "Id", "Id");
            return View();
        }

        // POST: Cuenta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Banco,Cuenta,TipoId,ClienteId,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] Cuentum cuentum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cuentum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", cuentum.ClienteId);
            ViewData["TipoId"] = new SelectList(_context.TipoCuenta, "Id", "Id", cuentum.TipoId);
            return View(cuentum);
        }

        // GET: Cuenta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cuenta == null)
            {
                return NotFound();
            }

            var cuentum = await _context.Cuenta.FindAsync(id);
            if (cuentum == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", cuentum.ClienteId);
            ViewData["TipoId"] = new SelectList(_context.TipoCuenta, "Id", "Id", cuentum.TipoId);
            return View(cuentum);
        }

        // POST: Cuenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Banco,Cuenta,TipoId,ClienteId,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] Cuentum cuentum)
        {
            if (id != cuentum.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuentum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentumExists(cuentum.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Id", cuentum.ClienteId);
            ViewData["TipoId"] = new SelectList(_context.TipoCuenta, "Id", "Id", cuentum.TipoId);
            return View(cuentum);
        }

        // GET: Cuenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cuenta == null)
            {
                return NotFound();
            }

            var cuentum = await _context.Cuenta
                .Include(c => c.Cliente)
                .Include(c => c.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cuentum == null)
            {
                return NotFound();
            }

            return View(cuentum);
        }

        // POST: Cuenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cuenta == null)
            {
                return Problem("Entity set 'easycreditContext.Cuenta'  is null.");
            }
            var cuentum = await _context.Cuenta.FindAsync(id);
            if (cuentum != null)
            {
                _context.Cuenta.Remove(cuentum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuentumExists(int id)
        {
          return (_context.Cuenta?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
