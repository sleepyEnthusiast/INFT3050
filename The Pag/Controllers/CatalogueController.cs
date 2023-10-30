using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Collections.Generic;
using System;
using System.Data;
using System.Security.Cryptography;
using The_Pag.Models;
using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using The_Pag.Classes;

namespace The_Pag.Controllers
{
    public class CatalogueController : Controller
    {
        private StoreDbContext context;

        private List<string> productTypes = new List<string> { "books", "movies", "games" };

        private List<string> bookGenres = new List<string>
        {
            "fiction", "historical-fiction", "fantasy-scifi", "ya", "humour", "crime", "mystery", "romance", "thriller"
        };

        private List<string> movieGenres = new List<string>
        {
            "drama", "comedy", "crime", "action", "horror", "family", "western", "documentary"
        };

        private List<string> gameGenres = new List<string>
        {
            "rpg", "musical", "puzzle", "strategy", "platform", "action-adventure", "racing", "stealth", "mmorpg", "survival", "simulation", "sports", "fps", "fighting"
        };

        private List<string> sortOptions = new List<string> { "name", "author", "published" };

        public CatalogueController(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Catalogue(string productType, string genre, string sortBy, string order)
        {
            if (productTypes.Contains(productType)) // Check if productType is Valid
            {
                int productID = productTypes.IndexOf(productType) + 1;
                int genreID = 0;
                List<string> selectedGenres = new List<string>();

                // Determine the list of genres based on the product type.
                if (productType == "books")
                {
                    selectedGenres = bookGenres;
                    ViewBag.genres = bookGenres;
                }
                else if (productType == "movies")
                {
                    selectedGenres = movieGenres;
                    ViewBag.genres = movieGenres;
                }
                else if (productType == "games")
                {
                    selectedGenres = gameGenres;
                    ViewBag.genres = gameGenres;
                }

                // Check if the provided genre is valid for the selected productType.
                if (selectedGenres.Contains(genre))
                {
                    genreID = selectedGenres.IndexOf(genre) + 1;

                    // Check if the provided sortBy and order are valid.
                    if (sortOptions.Contains(sortBy) && (order == "asc" || order == "desc"))
                    {
                        string query = $"SELECT * FROM Product WHERE Genre = @ProductType AND subGenre = @Genre ORDER BY {sortBy} {order}";
                        SqlParameter productParam = new SqlParameter("@ProductType", SqlDbType.Int);
                        SqlParameter genreParam = new SqlParameter("@Genre", SqlDbType.Int);

                        productParam.Value = productID;
                        genreParam.Value = genreID;

                        var display = context.Products.FromSqlRaw(query, productParam, genreParam).ToList();

                        ViewBag.type = productType;
                        ViewBag.genre = genre;//default filter options
                        ViewBag.sort = sortBy; 
                        ViewBag.order = order;

                        ViewBag.selectedGenres = selectedGenres;

                        ViewBag.sortOptions = sortOptions;

                        ViewBag.prodlist = display;
                        return View();
                    }
                }
            }
            // error
            return View();
        }   // Test Url: https://localhost:7289/Catalogue/Catalogue/movies/drama/name/desc
            // All the segments of the url must be filled for this method to activate. 


        public IActionResult Item(string ID)
        {
            if (ID == null) return View();
            string query = "SELECT * FROM Product WHERE ID = @ID";
            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = int.Parse(ID);

            var stocktake = context.Stocktakes.FromSqlRaw("SELECT * FROM Stocktake WHERE ItemId = @ID", idParam).ToList();
            ViewBag.stocktake = stocktake[0];

            var display = context.Products.FromSqlRaw(query, idParam).ToList();
            ViewBag.item = display;

            return View();
        } // https://localhost:7289/Catalogue/Item/1
    }
}
