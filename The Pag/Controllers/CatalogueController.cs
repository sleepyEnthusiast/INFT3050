using Microsoft.AspNetCore.Mvc;

namespace The_Pag.Controllers
{
    public class CatalogueController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Catalogue()
        {
            return View();
        }

        public IActionResult Item()
        {
            return View();
        }
    }
}
