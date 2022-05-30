using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using easycredit.Data;
using easycredit.Models;

namespace easycredit
{
    public class TipoClienteController : Controller
    {
        private readonly easycreditContext _context;

        public TipoClienteController(easycreditContext context)
        {
            _context = context;
        }

        // GET: TipoCliente
        public async Task<IActionResult> Index()
        {
              return _context.TipoClientes != null ? 
                          View(await _context.TipoClientes.ToListAsync()) :
                          Problem("Entity set 'easycreditContext.TipoClientes'  is null.");
        }

        // GET: TipoCliente/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TipoClientes == null)
            {
                return NotFound();
            }

            var tipoCliente = await _context.TipoClientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoCliente == null)
            {
                return NotFound();
            }

            return View(tipoCliente);
        }

        // GET: TipoCliente/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoCliente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tipo,Descripcion,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] TipoCliente tipoCliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoCliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoCliente);
        }

        // GET: TipoCliente/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TipoClientes == null)
            {
                return NotFound();
            }

            var tipoCliente = await _context.TipoClientes.FindAsync(id);
            if (tipoCliente == null)
            {
                return NotFound();
            }
            return View(tipoCliente);
        }

        // POST: TipoCliente/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tipo,Descripcion,FechaCreado,FechaEditado,FechaEliminado,UsuarioCreador,UsuarioEliminador,UsuarioEditor,Active")] TipoCliente tipoCliente)
        {
            if (id != tipoCliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoCliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoClienteExists(tipoCliente.Id))
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
            return View(tipoCliente);
        }

        // GET: TipoCliente/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TipoClientes == null)
            {
                return NotFound();
            }

            var tipoCliente = await _context.TipoClientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipoCliente == null)
            {
                return NotFound();
            }

            return View(tipoCliente);
        }

        // POST: TipoCliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TipoClientes == null)
            {
                return Problem("Entity set 'easycreditContext.TipoClientes'  is null.");
            }
            var tipoCliente = await _context.TipoClientes.FindAsync(id);
            if (tipoCliente != null)
            {
                _context.TipoClientes.Remove(tipoCliente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoClienteExists(int id)
        {
          return (_context.TipoClientes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
