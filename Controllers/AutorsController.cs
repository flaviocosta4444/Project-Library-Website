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
    public class AutorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AutorsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webHostEnvironment = environment;  
        }

        // GET: Autors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Autores.ToListAsync());
        }

        // GET: Autors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .FirstOrDefaultAsync(m => m.AutorId == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AutorId,Nome,Foto")] AutorViewModel autor)
        {

            //Validar as extensões dos files
            var FotoExtensions = new[] { ".jpg", ".jpeg", ".png" };

            var extensions = Path.GetExtension(autor.Foto.FileName).ToLower();

            if (!FotoExtensions.Contains(extensions))
            {
                ModelState.AddModelError("Foto", "Extensão inválida. Use .jpg, .jpeg ou .png");
            }
            extensions = Path.GetExtension(autor.Foto.FileName).ToLower();

            if (ModelState.IsValid)
            {
                var newAutor = new Autor();
                newAutor.Nome = autor.Nome;
                newAutor.Foto = autor.Foto.FileName;

                //Salvar file
                string FotoAutorPath = Path.GetFileName(autor.Foto.FileName);
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "FotoAutor", FotoAutorPath);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await autor.Foto.CopyToAsync(stream);
                }

                _context.Add(newAutor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // GET: Autors/Edit/5
        public async Task<IActionResult> Edit(int? id, IFormFile Foto)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AutorId,Nome,LivroId")] Autor autor, IFormFile? Foto)
        {
            if (id != autor.AutorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Recupera o autor existente no banco de dados
                    var autorExistente = await _context.Autores.FindAsync(id);

                    if (autorExistente == null)
                    {
                        return NotFound();
                    }

                    // Atualiza os campos editados
                    autorExistente.Nome = autor.Nome;

                    // Se uma nova foto for enviada
                    if (Foto != null && Foto.Length > 0)
                    {
                        // Define o nome do arquivo (pode ser o nome original ou gerado dinamicamente)
                        var fileName = Path.GetFileName(Foto.FileName);

                        // Caminho para salvar a foto no diretório "wwwroot/FotoAutor"
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "FotoAutor", fileName);

                        // Salva o arquivo no sistema de arquivos
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Foto.CopyToAsync(stream);
                        }

                        // Atualiza o campo "Foto" com o nome do arquivo salvo
                        autorExistente.Foto = fileName;
                    }

                    // Atualiza os dados do autor no banco de dados
                    _context.Update(autorExistente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.AutorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Redireciona para a página de índice (lista de autores)
                return RedirectToAction(nameof(Index));
            }

            // Retorna a view com os dados enviados em caso de erro de validação
            return View(autor);
        }

        // GET: Autors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autores
                .FirstOrDefaultAsync(m => m.AutorId == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.Autores.FindAsync(id);
            if (autor != null)
            {
                _context.Autores.Remove(autor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.Autores.Any(e => e.AutorId == id);
        }
    }
}
