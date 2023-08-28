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
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Order_Details()
        {
            return View();
        }

        public IActionResult Order_History()
        {
            return View();
        }

        public IActionResult Update_Order_Details()
        {
            return View();
        }
    }
}
