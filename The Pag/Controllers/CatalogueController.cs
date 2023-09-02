using Microsoft.AspNetCore.Mvc;
using The_Pag.Models;

namespace The_Pag.Controllers
{
    public class CatalogueController : Controller
    {
        private StoreDbContext context;

        public CatalogueController(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Catalogue()
        {
            IQueryable<Product> prodlist = context.Products; // Query initiation
            var display = prodlist.ToList();  // Query execution, creates list of models
            ViewBag.prodlist = display; // Puts results into the BAG

            return View();
        }

        public IActionResult Item()
        {
            return View();
        }
    }
}
