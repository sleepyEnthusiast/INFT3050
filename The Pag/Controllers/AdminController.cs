using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.Pipelines;
using System.Runtime.Intrinsics.X86;
using The_Pag.Classes;
using The_Pag.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace The_Pag.Controllers
{
    public class AdminController : Controller
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
        public AdminController(StoreDbContext ctx)
        {
            context = ctx;
        }
        
        public IActionResult Edit_Item(string ID)
        {
            if (ID == null) return RedirectToAction("Item_Management");
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return RedirectToAction("Item_Management");
            
            string query = "SELECT * FROM Product WHERE ID = @ID";
            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = int.Parse(ID);
            var display = context.Products.FromSqlRaw(query, idParam).ToList();

            ViewBag.item = display[0];

            int productID = display[0].Genre.Value;
            int genreID = display[0].SubGenre.Value;

            List<string> selectedGenre = new List<string>();

            switch (productID)
            {
                case 1:
                    selectedGenre = bookGenres;
                    break;
                case 2:
                    selectedGenre = movieGenres;
                    break;
                case 3:
                    selectedGenre = gameGenres;
                    break;
            }

            var stocktake = context.Stocktakes.FromSqlRaw("SELECT * FROM Stocktake WHERE ProductId = @ID", idParam).ToList();
            ViewBag.stocktake = stocktake[0];

            ViewBag.genre = selectedGenre.ElementAt(genreID - 1);
            ViewBag.genreList = selectedGenre;
            ViewBag.product = productTypes.ElementAt(productID - 1);
            ViewBag.productList = productTypes;

            

            return View();
        }

        public IActionResult Add_Item(string ID)
        {
            if (ID == null) return Redirect("~/");
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return RedirectToAction("Item_Management");
            if (CookieConfirm.HavePermission() != 3) return RedirectToAction("Item_Management");

            List<string> selectedGenre = new List<string>();
            switch (Convert.ToInt32(ID))
            {
                case 1:
                    selectedGenre = bookGenres;
                    break;
                case 2:
                    selectedGenre = movieGenres;
                    break;
                case 3:
                    selectedGenre = gameGenres;
                    break;
            }

            var sources = context.Sources.FromSqlRaw("SELECT * FROM Source").ToList();
            ViewBag.sources = sources;

            ViewBag.genre = ID;
            ViewBag.genreList = selectedGenre;

            return View();
        }
        public IActionResult Edit_Item_Action(IFormCollection input) // The actual item editing
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return RedirectToAction("Item_Management");

            string productquery =
            "UPDATE Product " +
                "SET " +
                    "Name = @Name," +
                    "Author = @Author, " +
                    "Description = @Description, " +
                    "Genre = @Genre, " +
                    "subGenre = @SubGenre, " +
                    "Published = @Published, " +
                    "LastUpdatedBy = @User, " +
                    "LastUpdated = GETDATE() " +
            "WHERE ID = @ID ;";

            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = Convert.ToInt32(input["ID"]);

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.VarChar);
            nameParam.Value = Convert.ToString(input["Name"]);

            SqlParameter authorParam = new SqlParameter("@Author", SqlDbType.VarChar);
            authorParam.Value = Convert.ToString(input["Author"]);

            SqlParameter descParam = new SqlParameter("@Description", SqlDbType.VarChar);
            descParam.Value = Convert.ToString(input["Description"]);

            SqlParameter genreParam = new SqlParameter("@Genre", SqlDbType.Int);
            int productID = Convert.ToInt32(productTypes.IndexOf(input["Genre"]) + 1);
            genreParam.Value = productID;

            SqlParameter subParam = new SqlParameter("@SubGenre", SqlDbType.Int);

            switch (productID)
            {
                case 1:
                    subParam.Value = Convert.ToInt32(bookGenres.IndexOf(input["subGenre"]) + 1);
                    break;
                case 2:
                    subParam.Value = Convert.ToInt32(movieGenres.IndexOf(input["subGenre"]) + 1);
                    break;
                case 3:
                    subParam.Value = Convert.ToInt32(gameGenres.IndexOf(input["subGenre"]) + 1);
                    break;
            }

            SqlParameter publishParam = new SqlParameter("@Published", SqlDbType.Date);
            publishParam.Value = DateTime.Parse(input["publishedDate"]);

            SqlParameter userParam = new SqlParameter("@User", SqlDbType.VarChar);
            userParam.Value = "storeManager";

            string stocktakequery =
            "UPDATE StockTake " +
                "SET " +
                    "Price = @Price, " +
                    "Quantity = @Quantity " +
            "WHERE ProductId = @ID;";
            
            SqlParameter priceParam = new SqlParameter("@Price", SqlDbType.Float);
            priceParam.Value = Convert.ToDecimal(input["Price"]);

            SqlParameter quantityParam = new SqlParameter("@Quantity", SqlDbType.Int);
            quantityParam.Value = Convert.ToInt32(input["Quantity"]);

            context.Database.ExecuteSqlRaw(productquery, nameParam, authorParam, descParam, genreParam, subParam, publishParam, userParam, idParam);
            
            context.Database.ExecuteSqlRaw(stocktakequery, priceParam, quantityParam, idParam);

            return RedirectToAction("Item_Management");
        }
        
        public IActionResult Add_Item_Action(IFormCollection input)
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return RedirectToAction("Item_Management");

            string productquery = "INSERT INTO Product (Name, Author, Description, Genre, subGenre, Published, LastUpdatedBy, LastUpdated) " +
                                  "OUTPUT Inserted.ID, Inserted.Name, Inserted.Author, Inserted.Description, Inserted.Genre, Inserted.subGenre, Inserted.Published, Inserted.LastUpdatedby,Inserted.LastUpdated " +
                                  "VALUES (@Name, @Author, @Description, @Genre, @SubGenre, @Published, @User, GETDATE());";

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.VarChar);
            nameParam.Value = Convert.ToString(input["Name"]);

            SqlParameter authorParam = new SqlParameter("@Author", SqlDbType.VarChar);
            authorParam.Value = Convert.ToString(input["Author"]);

            SqlParameter descParam = new SqlParameter("@Description", SqlDbType.VarChar);
            descParam.Value = Convert.ToString(input["Description"]);

            SqlParameter genreParam = new SqlParameter("@Genre", SqlDbType.Int);
            int productID = Convert.ToInt32(input["Genre"]);
            genreParam.Value = productID;

            SqlParameter subParam = new SqlParameter("@SubGenre", SqlDbType.Int);

            switch (productID)
            {
                case 1:
                    subParam.Value = Convert.ToInt32(bookGenres.IndexOf(input["subGenre"]) + 1);
                    break;
                case 2:
                    subParam.Value = Convert.ToInt32(movieGenres.IndexOf(input["subGenre"]) + 1);
                    break;
                case 3:
                    subParam.Value = Convert.ToInt32(gameGenres.IndexOf(input["subGenre"]) + 1);
                    break;
            }

            SqlParameter publishParam = new SqlParameter("@Published", SqlDbType.Date);
            if (DateTime.TryParse(input["publishedDate"], out DateTime publishDate))
            {
                publishParam.Value = publishDate;
            }
            else
            {
                publishParam.Value = DBNull.Value;
            }

            SqlParameter userParam = new SqlParameter("@User", SqlDbType.VarChar);
            userParam.Value = "storeManager";

            string stocktakequery = "INSERT INTO StockTake(SourceId, ProductId, Price, Quantity) " +
                                    "VALUES(@Source, @ID, @Price, @Quantity);";

            SqlParameter priceParam = new SqlParameter("@Price", SqlDbType.Float);
            if (decimal.TryParse(input["Price"], out decimal priceValue))
            {
                priceParam.Value = priceValue;
            }
            else
            {
                priceParam.Value = DBNull.Value;
            }

            SqlParameter quantityParam = new SqlParameter("@Quantity", SqlDbType.Int);
            if (int.TryParse(input["Quantity"], out int quantity))
            {
                quantityParam.Value = quantity;
            }
            else
            {
                quantityParam.Value = DBNull.Value;
            }

            SqlParameter sourcenameParam = new SqlParameter("@sourceName", SqlDbType.VarChar);
            sourcenameParam.Value = Convert.ToString(input["source"]);

            var source = context.Sources.FromSqlRaw("SELECT * FROM Source WHERE Source_name = @sourceName", sourcenameParam).ToList();

            SqlParameter sourceParam = new SqlParameter("@Source", SqlDbType.Int);
            sourceParam.Value = source[0].Sourceid;

            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            var newEntry = context.Products.FromSqlRaw(productquery, nameParam, authorParam, descParam, genreParam, subParam, publishParam, userParam).ToList();
            idParam.Value = Convert.ToInt32(newEntry[0].Id);

            context.Database.ExecuteSqlRaw(stocktakequery, sourceParam, idParam, priceParam, quantityParam);

            return RedirectToAction("Item_Management");
        }
        public IActionResult Delete_Item(string ID)
        {
            if (ID == null) return RedirectToAction("Item_Management");
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return RedirectToAction("Item_Management");

            string productquery =   "DELETE FROM Product " +
                                    "WHERE ID = @ID;";
            string stockquery =     "DELETE FROM StockTake " +
                                    "WHERE ProductId = @ID;";
            string orderquery =     "DELETE FROM ProductsInOrders " +
                                    "WHERE produktId = @ID;";

            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = Convert.ToInt32(ID);

            context.Database.ExecuteSqlRaw(orderquery, idParam);
            context.Database.ExecuteSqlRaw(stockquery, idParam);
            context.Database.ExecuteSqlRaw(productquery, idParam);
            

            return RedirectToAction("Item_Management");
        }
        public IActionResult Edit_User(string ID)
        {
            if (ID == null) return RedirectToAction("User_Management");
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return Redirect("~/");

            if (CookieConfirm.GetUserID(Request.Cookies["TokenCookie"]) == Convert.ToInt32(ID)) return RedirectToAction("User_Management"); // user cant edit their own entry

            string query = "SELECT * FROM [User] WHERE UserID = @ID;";

            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = int.Parse(ID);

            var display = context.Users.FromSqlRaw(query, idParam).ToList();

            ViewBag.user = display[0];

            return View();

        }// This is where i realised that the primary key for user is UserName not ID, so Delete user uses UserName Instead

        public IActionResult Add_User()
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() == 1 || CookieConfirm.HavePermission() == 0) return Redirect("~/");


            return View();
        }
        public IActionResult Edit_User_Action(IFormCollection input)
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return Redirect("~/");
            SqlParameter emailParam = new SqlParameter("@Email", SqlDbType.NVarChar);
            emailParam.Value = Convert.ToString(input["email"]);

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.NVarChar);
            nameParam.Value = Convert.ToString(input["name"]);

            SqlParameter adminParam = new SqlParameter("@Admin", SqlDbType.Bit);
            int admin = Convert.ToInt32(input["isAdmin"]);

            if (admin == 1)
            {
                adminParam.Value = true;
            } else {
                adminParam.Value = false;
            }
            
            SqlParameter userParam = new SqlParameter("@User", SqlDbType.NVarChar);
            userParam.Value = Convert.ToString(input["username"]);

            string query = "UPDATE [User] " +
                           "SET " +
                            "Email = @Email, " +
                            "Name = @Name, " +
                            "isAdmin = @Admin " +
                           "WHERE UserName = @User;";

            context.Database.ExecuteSqlRaw(query, emailParam, nameParam, adminParam, userParam);

            return RedirectToAction("User_Management");
        }

        public IActionResult Add_User_Action(IFormCollection input)
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return Redirect("~/");

            SqlParameter userParam = new SqlParameter("@User", SqlDbType.NVarChar);
            userParam.Value = Convert.ToString(input["username"]);

            SqlParameter emailParam = new SqlParameter("@Email", SqlDbType.NVarChar);
            emailParam.Value = Convert.ToString(input["email"]);

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.NVarChar);
            nameParam.Value = Convert.ToString(input["name"]);

            SqlParameter adminParam = new SqlParameter("@Admin", SqlDbType.Bit);
            int admin = Convert.ToInt32(input["isAdmin"]);

            if (admin == 1)
            {
                adminParam.Value = true;
            }
            else
            {
                adminParam.Value = false;
            }

            string query =  "INSERT INTO [User] (UserName, Email, Name, isAdmin, Salt, HashPW) " +
                            "VALUES (@User, @Email, @Name, @Admin, 'Not Implemented', 'Not Implemented');";

            context.Database.ExecuteSqlRaw(query, userParam, emailParam, nameParam, adminParam);

            return RedirectToAction("User_Management");
        }
        public IActionResult Delete_User(string ID)
        {
            if (ID == null) return RedirectToAction("User_Management");
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return Redirect("~/");

            SqlParameter userParam = new SqlParameter("@User", SqlDbType.NVarChar);
            userParam.Value = Convert.ToString(ID);

            context.Database.ExecuteSqlRaw( "DELETE FROM [User] " +
                                            "WHERE UserName = @User;",
                                            userParam);

            return RedirectToAction("User_Management");

        }

        public IActionResult Edit_Patron(string ID)
        {
            if (ID == null) return RedirectToAction("Patron_Management");
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return Redirect("~/");

            string query = "SELECT * FROM [Patron] WHERE UserID = @ID;";

            SqlParameter idParam = new SqlParameter("@ID", SqlDbType.Int);
            idParam.Value = int.Parse(ID);

            var display = context.Patrons.FromSqlRaw(query, idParam).ToList();

            ViewBag.user = display[0];

            return View();

        }

        public IActionResult Add_Patron()
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() == 1 || CookieConfirm.HavePermission() == 0) return Redirect("~/");


            return View();
        }
        public IActionResult Edit_Patron_Action(IFormCollection input)
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return Redirect("~/");
            SqlParameter emailParam = new SqlParameter("@Email", SqlDbType.NVarChar);
            emailParam.Value = Convert.ToString(input["email"]);

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.NVarChar);
            nameParam.Value = Convert.ToString(input["name"]);

            SqlParameter userParam = new SqlParameter("@User", SqlDbType.NVarChar);
            userParam.Value = Convert.ToString(input["ID"]);

            string query = "UPDATE [Patrons] " +
                           "SET " +
                            "Email = @Email, " +
                            "Name = @Name, " +
                           "WHERE UserID = @User;";

            context.Database.ExecuteSqlRaw(query, emailParam, nameParam, userParam);

            return RedirectToAction("Patron_Management");
        }

        public IActionResult Add_Patron_Action(IFormCollection input)
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return Redirect("~/");
            
            SqlParameter emailParam = new SqlParameter("@Email", SqlDbType.NVarChar);
            emailParam.Value = Convert.ToString(input["email"]);

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.NVarChar);
            nameParam.Value = Convert.ToString(input["name"]);

            string query =  "INSERT INTO [Patrons] (Email, Name, Salt, HashPW) " +
                            "VALUES (@Email, @Name, 'Not Implemented', 'Not Implemented');";

            context.Database.ExecuteSqlRaw(query, emailParam, nameParam);

            return RedirectToAction("Patron_Management");
        }
        public IActionResult Delete_Patron(string ID)
        {
            if (ID == null) return RedirectToAction("Patron_Management");
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() != 3) return Redirect("~/");

            SqlParameter userParam = new SqlParameter("@User", SqlDbType.Int);
            userParam.Value = Convert.ToInt32(ID);

            context.Database.ExecuteSqlRaw("DELETE FROM [Patron] " +
                                            "WHERE UserID = @User;",
                                            userParam);

            return RedirectToAction("Patron_Management");

        }
        public IActionResult Item_Management()
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            if (CookieConfirm.HavePermission() == 1 || CookieConfirm.HavePermission() == 0) return Redirect("~/");
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

        public IActionResult Patron_Management()
        {
            IQueryable<Patron> userList = context.Patrons; // Query initiation
            var listofusers = userList.ToList();  // Query execution, creates list of models
            ViewBag.userList = listofusers; // Puts results into the BAG

            return View();
        }

    }
}
