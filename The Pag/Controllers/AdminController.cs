using Microsoft.AspNetCore.Mvc;
using The_Pag.Models;

namespace The_Pag.Controllers
{
    public class AdminController : Controller
    {
        private StoreDbContext context;

        public AdminController(StoreDbContext ctx)
        {
            context = ctx;
        }
        
        public IActionResult Edit_Item()
        {
            return View();
        }

        public IActionResult Edit_User()
        {
            return View();
        }

        public IActionResult Item_Management()
        {
            IQueryable<Product> prodlist = context.Products; // Query initiation
            var list = prodlist.ToList();  // Query execution, creates list of models
            ViewBag.prodlist = list; // Puts results into the BAG

            return View();
        }

        public IActionResult User_Management()
        {
            IQueryable<User> userList = context.Users; // Query initiation
            var listofusers = userList.ToList();  // Query execution, creates list of models
            ViewBag.userList = listofusers ; // Puts results into the BAG

            return View();
        }


    }
}
