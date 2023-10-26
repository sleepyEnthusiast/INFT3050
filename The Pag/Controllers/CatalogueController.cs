using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Data;
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

        public IActionResult Catalogue(string productType, string genre, string sortOrder)
        {
            // Construct the SQL query with a parameterized query to prevent SQL injection
            string query = "SELECT * FROM Product WHERE Genre = @Genre";
            SqlParameter genreParam = new SqlParameter("@Genre", SqlDbType.Int);

            switch (productType)
            {
                case "books":
                    genreParam.Value = 1;
                    break;
                case "movies":
                    genreParam.Value = 2;
                    break;
                case "games":
                    genreParam.Value = 3;
                    break;
                default:
                    // Handle the default case (error or redirection)
                    return View();
            }

            var display = context.Products.FromSqlRaw(query, genreParam).ToList();
            ViewBag.prodlist = display;



            return View();
        }   // Test Url: https://localhost:7289/Catalogue/Catalogue/movies/1/sortOrderValue
            // all the segments of the url must be filled for this method to activate. 


        public IActionResult Item()
        {
            return View();
        }
    }
}
