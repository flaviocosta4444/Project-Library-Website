using B_LEI.Data;
using B_LEI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B_LEI.Controllers
{
    [Authorize(Roles = "admin")]
    public class GerirUtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;

        public GerirUtilizadoresController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }
        // GET: GerirUtilizadoresController
        public ActionResult Index()
        {
            // Listar ApplicationUser 
            return View(_context.Users.ToList());
        }

        // GET: GerirUtilizadoresController/Details/5
        public async Task<ActionResult> Details(string id) // Aqui é comum usar string como tipo de id no Identity
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // Obter as roles do usuário
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;

            return View(user);
        }
        // Ação para bloquear o usuário com um motivo
        public async Task<IActionResult> Bloquear(string id, string motivo)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Obter as roles do usuário
            var roles = await _userManager.GetRolesAsync(user);

            // Passar as roles para a view
            ViewBag.Roles = roles;

            // Verificar se o usuário tem a role "leitor" ou "bibliotecario"
            if (!(roles.Contains("leitor") || roles.Contains("bibliotecario")))
            {
                // Se o usuário não tiver as roles necessárias, exibe um erro
                ModelState.AddModelError(string.Empty, "O usuário não pode ser bloqueado, pois não tem a role necessária.");
                return RedirectToAction(nameof(Details), new { id = user.Id });
            }

            // Bloquear o usuário definindo LockoutEnd para 100 anos no futuro
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);

            // Definir o motivo do bloqueio
            user.LockoutReason = motivo;

            _context.Update(user);
            await _context.SaveChangesAsync();

            // Redirecionar de volta para a página de detalhes do usuário
            return RedirectToAction(nameof(Details), new { id = user.Id });
        }

        // Ação para desbloquear o usuário
        public async Task<IActionResult> Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }


            // Remover o bloqueio definindo LockoutEnd como null
            user.LockoutEnd = null;
            user.LockoutReason = null; // Limpar o motivo de bloqueio

            _context.Update(user);
            await _context.SaveChangesAsync();

            // Redirecionar de volta para a página de detalhes do usuário
            return RedirectToAction(nameof(Details), new { id = user.Id });
        }

        // GET: GerirUtilizadoresController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GerirUtilizadoresController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                // Criar o novo usuário
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Name = model.Name,
                    DataCriada = DateTime.Now
                };

                // Criar o usuário no banco de dados com a senha padrão
                var result = await _userManager.CreateAsync(user, "Defaultadmin123!"); // Senha padrão
                if (result.Succeeded)
                {
                    // Garantir que a role "admin" exista
                    if (!await _roleManager.RoleExistsAsync("admin"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                    }

                    // Adicionar o usuário à role "admin"
                    await _userManager.AddToRoleAsync(user, "admin");

                    // Gerar o token de confirmação de e-mail
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    // Criar o link de confirmação com o token gerado
                    var confirmationLink = Url.Action("ConfirmEmail", "GerirUtilizadores",
                        new { userId = user.Id, token = token }, protocol: HttpContext.Request.Scheme);

                    // Enviar o link de confirmação de e-mail para o usuário
                    await _emailSender.SendEmailAsync(user.Email, "Confirme seu e-mail",
                        $"Clique <a href='{confirmationLink}'>aqui</a> para confirmar seu e-mail.");

                    // Redireciona o usuário para a página de lista após o envio do e-mail
                    return RedirectToAction(nameof(Index));
                }

                // Se houver erros, adiciona-os ao ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Se o modelo não for válido, retorna a mesma página
            return View(model);
        }

        // Método para confirmar o e-mail
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            // Verifica se os parâmetros são válidos
            if (userId == null || token == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Busca o usuário pelo ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            // Confirma o e-mail usando o token gerado
            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                // O e-mail foi confirmado com sucesso
                return View("ConfirmEmail"); // Você pode exibir uma página de sucesso
            }
            else
            {
                // Se houver falha na confirmação
                return View("Error");
            }
        }

        // GET: GerirUtilizadoresController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Verificar se o usuário logado é um administrador
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                var currentUserRoles = await _userManager.GetRolesAsync(currentUser);
                var userRoles = await _userManager.GetRolesAsync(user);

                // Se o usuário logado for administrador e está tentando editar outro administrador
                if (currentUserRoles.Contains("admin") && userRoles.Contains("admin"))
                {
                    // Impedir que o administrador edite outro administrador
                    TempData["Error"] = "Você não pode editar outro administrador.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(user);
        }

        // POST: GerirUtilizadoresController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ApplicationUser model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        return NotFound();
                    }

                    // Atualizar os dados do usuário
                    user.Name = model.Name;
                    user.Morada = model.Morada;
                    user.PhoneNumber = model.PhoneNumber;

                    // Não alteramos UserName e Email aqui, pois são campos readonly

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(model);
        }

        private bool UserExists(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result; // Sincronização para obter o usuário
            return user != null;
        }
    }
}
