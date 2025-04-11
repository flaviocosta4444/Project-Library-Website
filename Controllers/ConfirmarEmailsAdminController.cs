using B_LEI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace B_LEI.Controllers
{
    [Authorize(Roles = "admin")]
    public class ConfirmarEmailsAdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ConfirmarEmailsAdminController(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // Exibe a lista de bibliotecários pendentes
        public async Task<IActionResult> IndexAsync()
        {
            // Busca os usuários com a role "Bibliotecario"
            var bibliotecarios = await _userManager.GetUsersInRoleAsync("Bibliotecario");

            // Retorna para a View com os dados dos Bibliotecários
            return View(bibliotecarios);
        }

        // Aprova um bibliotecário
        [HttpPost]
        public async Task<IActionResult> ApproveBibliotecario(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "ID do usuário não fornecido.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Usuário não encontrado.";
                return RedirectToAction("Index");
            }

            if (user.IsEmailConfirmedByAdmin)
            {
                TempData["ErrorMessage"] = "Este usuário já foi aprovado.";
                return RedirectToAction("Index");
            }

            // Marca o usuário como aprovado e registra o administrador que fez a aprovação
            user.IsEmailConfirmedByAdmin = true;
            user.VerifiedByAdminUsername = User.Identity.Name; // Registra o nome do administrador

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Bibliotecário aprovado com sucesso.";

                // Notifica o bibliotecário por e-mail
                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Aprovação do E-mail",
                    $"Olá {user.Name},<br/><br/>Seu cadastro foi aprovado pelo administrador. Você agora pode acessar o site.<br/><br/>Atenciosamente,<br/>Equipe B_LEI"
                );
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao aprovar o bibliotecário.";
            }

            return RedirectToAction("Index");
        }
    }
}
