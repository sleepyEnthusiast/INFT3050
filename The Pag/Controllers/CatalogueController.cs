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

        public IActionResult Catalogue(string productType, string genre, string sortBy, string order)
        {
            // Paramater query construction to stop injection.
            string query = "SELECT * FROM Product WHERE Genre = @ProductType AND subGenre = @Genre ORDER BY ";
            SqlParameter productParam = new SqlParameter("@ProductType", SqlDbType.Int);
            SqlParameter genreParam = new SqlParameter("@Genre", SqlDbType.Int);

            switch (sortBy)
            {
                case "name":
                    query += "Name";
                    break;
                case "author":
                    query += "Author";
                    break;
                case "published":
                    query += "Published";
                    break;
                default:
                    return View();
            }

            switch (order)
            {
                case "asc":
                    query += " ASC";
                    break;
                case "desc":
                    query += " DESC";
                    break;
                default:
                    // Handle the default case (error or redirection)
                    return View();
            }

            switch (productType)
            {
                case "books":
                    switch (genre.Trim())
                    {
                        case "fiction":
                            genreParam.Value = 1;
                            break;
                        case "historical-fiction":
                            genreParam.Value = 2;
                            break;
                        case "fantasy-scifi":
                            genreParam.Value = 3;
                            break;
                        case "ya":
                            genreParam.Value = 4;
                            break;
                        case "humour":
                            genreParam.Value = 5;
                            break;
                        case "crime":
                            genreParam.Value = 6;
                            break;
                        case "mystery":
                            genreParam.Value = 7;
                            break;
                        case "romance":
                            genreParam.Value = 8;
                            break;
                        case "thriller":
                            genreParam.Value = 9;
                            break;
                        default:
                            // Handle the default case (error or redirection)
                            return View();
                    }
                    
                    productParam.Value = 1;
                    break;
                case "movies":
                    switch (genre.Trim())
                    {
                        case "drama":
                            genreParam.Value = 1;
                            break;
                        case "comedy":
                            genreParam.Value = 2;
                            break;
                        case "crime":
                            genreParam.Value = 3;
                            break;
                        case "action":
                            genreParam.Value = 4;
                            break;
                        case "horror":
                            genreParam.Value = 5;
                            break;
                        case "family":
                            genreParam.Value = 6;
                            break;
                        case "western":
                            genreParam.Value = 7;
                            break;
                        case "documentary":
                            genreParam.Value = 8;
                            break;
                        default:
                            // Handle the default case (error or redirection)
                            return View();
                    }

                    productParam.Value = 2;
                    break;
                case "games":
                    switch (genre.Trim())
                    {
                        case "rpg":
                            genreParam.Value = 1;
                            break;
                        case "musical":
                            genreParam.Value = 2;
                            break;
                        case "puzzle":
                            genreParam.Value = 3;
                            break;
                        case "strategy":
                            genreParam.Value = 4;
                            break;
                        case "platform":
                            genreParam.Value = 5;
                            break;
                        case "action-adventure":
                            genreParam.Value = 6;
                            break;
                        case "racing":
                            genreParam.Value = 7;
                            break;
                        case "stealth":
                            genreParam.Value = 8;
                            break;
                        case "mmorpg":
                            genreParam.Value = 9;
                            break;
                        case "survival":
                            genreParam.Value = 10;
                            break;
                        case "simulation":
                            genreParam.Value = 11;
                            break;
                        case "sports":
                            genreParam.Value = 12;
                            break;
                        case "fps":
                            genreParam.Value = 13;
                            break;
                        case "fighting":
                            genreParam.Value = 14;
                            break;
                        default:
                            // Handle the default case (error or redirection)
                            return View();
                    }

                    productParam.Value = 3;
                    break;
                default:
                    // Handle the default case (error or redirection)
                    return View();
            }

            var display = context.Products.FromSqlRaw(query, productParam, genreParam).ToList();
            ViewBag.prodlist = display;


            return View();
        }   // Test Url: https://localhost:7289/Catalogue/Catalogue/movies/drama/name/desc
            // all the segments of the url must be filled for this method to activate. 


        public IActionResult Item(string ID)
        {
            if (ID == null) return View();
            string query = "SELECT * FROM Product WHERE ID = @ID";
            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = int.Parse(ID);
            ViewBag.name = query;
            var display = context.Products.FromSqlRaw(query, idParam).ToList();
            ViewBag.item = display;

            return View();
        } // https://localhost:7289/Catalogue/Item/1
    }
}
