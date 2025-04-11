using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using B_LEI.Data;
using B_LEI.Models;
using Microsoft.AspNetCore.Authorization;

namespace B_LEI.Controllers
{
    [Authorize(Roles = "bibliotecario")]
    public class EditorasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EditorasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Editoras
        public async Task<IActionResult> Index()
        {
            return View(await _context.Editoras.ToListAsync());
        }

        // GET: Editoras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editora = await _context.Editoras
                .FirstOrDefaultAsync(m => m.EditoraId == id);
            if (editora == null)
            {
                return NotFound();
            }

            return View(editora);
        }

        // GET: Editoras/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Editoras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EditoraId,Nome,Descricao,LivroId")] Editora editora)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editora);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editora);
        }

        // GET: Editoras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editora = await _context.Editoras.FindAsync(id);
            if (editora == null)
            {
                return NotFound();
            }
            return View(editora);
        }

        // POST: Editoras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EditoraId,Nome,Descricao,LivroId")] Editora editora)
        {
            if (id != editora.EditoraId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editora);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditoraExists(editora.EditoraId))
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
            return View(editora);
        }

        // GET: Editoras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editora = await _context.Editoras
                .FirstOrDefaultAsync(m => m.EditoraId == id);
            if (editora == null)
            {
                return NotFound();
            }

            return View(editora);
        }

        // POST: Editoras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var editora = await _context.Editoras.FindAsync(id);
            if (editora != null)
            {
                _context.Editoras.Remove(editora);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EditoraExists(int id)
        {
            return _context.Editoras.Any(e => e.EditoraId == id);
        }
    }
}
