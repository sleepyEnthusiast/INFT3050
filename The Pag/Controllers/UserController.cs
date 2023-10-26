using Microsoft.AspNetCore.Mvc;
using The_Pag.Models;

namespace The_Pag.Controllers
{
    public class UserController : Controller
    {

        private StoreDbContext context;

        public UserController(StoreDbContext ctx)
        {
            context = ctx;
        }

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

            IQueryable<User> userList = context.Users; // Query initiation
            var listofusers = userList.ToList();  // Query execution, creates list of models
            ViewBag.userList = listofusers; // Puts results into the BAG

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
