using Microsoft.AspNetCore.Mvc;

namespace The_Pag.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Account()
        {
            return View();
        }
        public IActionResult Account_Create()
        {
            return View();
        }
    }
}
