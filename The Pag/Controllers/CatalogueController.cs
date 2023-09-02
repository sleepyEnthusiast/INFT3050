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
            IQueryable<Product> aquery = context.Products; // Query initiation
            var display = aquery.ToList();  // Query execution, creates list of models
            ViewBag.Test = display; // Puts results into the BAG
            return View();
        }

        public IActionResult Item()
        {
            return View();
        }
    }
}
