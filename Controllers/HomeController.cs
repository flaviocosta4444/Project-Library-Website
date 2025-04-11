using B_LEI.Data;
using B_LEI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;

namespace B_LEI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        // Injeta o contexto do Entity Framework no construtor
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // P�gina principal com listagem e filtros
        public IActionResult Index(string search, string categoria)
        {
            // Conta as requisi��es em atraso para o bibliotec�rio ver no layout
            ViewBag.RequisicoesAtrasadas = ContarRequisicoesAtrasadas();

            // Consulta inicial para obter os livros
            var livros = _context.Livros
                .Include(l => l.Autor)
                .Include(l => l.Categoria)
                .AsQueryable();

            // Filtro por categoria
            if (!string.IsNullOrEmpty(categoria))
            {
                livros = livros.Where(l => l.Categoria != null && l.Categoria.Nome == categoria);
            }

            // Filtro por busca (t�tulo ou autor)
            if (!string.IsNullOrEmpty(search))
            {
                livros = livros.Where(l =>
                    (l.Titulo != null && l.Titulo.Contains(search)) ||
                    (l.Autor != null && l.Autor.Nome != null && l.Autor.Nome.Contains(search))
                );
            }

            // Envia as categorias distintas para a View (para exibir como filtro)
            ViewBag.Categorias = _context.Livros
                .Select(l => l.Categoria)
                .Distinct()
                .ToList();

            // Retorna os livros filtrados para a View
            return View(livros.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Exibe p�gina de erro padr�o
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Voc� pode ter um ErrorViewModel para mostrar detalhes de erro
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Exibe as requisi��es do utilizador logado
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MinhasRequisicoes()
        {
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var requisicoes = await _context.Requisicoes
                .Include(r => r.Livro)
                .Where(r => r.UserId == userId)
                .ToListAsync();

            return View(requisicoes);
        }

        /// <summary>
        /// Conta quantas requisi��es est�o atrasadas (DataEntrega < hoje e sem DataDevolucao).
        /// </summary>
        private int ContarRequisicoesAtrasadas()
        {
            return _context.Requisicoes
                .Where(r => r.DataEntrega.HasValue
                            && r.DataEntrega.Value < DateTime.Now
                            && !r.DataDevolucao.HasValue)
                .Count();
        }
    }
}

