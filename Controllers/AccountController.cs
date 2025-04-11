using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B_LEI.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
