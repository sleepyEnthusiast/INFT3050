using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using The_Pag.Models;
using System.Security.Cryptography;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using The_Pag.Classes;
using System.Net;

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
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");

            string token = Request.Cookies["TokenCookie"];

            SqlParameter userParam = new SqlParameter("@User", SqlDbType.Int);
            userParam.Value = CookieConfirm.GetUserID(token);

            bool PatronOrUser = CookieConfirm.GetUserOrPatron(token);

            if (PatronOrUser)
            {
                var user = context.Users.FromSqlRaw("SELECT * FROM [User] WHERE UserId = @User;", userParam).ToList();
                ViewBag.user = user[0];
            } else
            {
                var user = context.Patrons.FromSqlRaw("SELECT * FROM [Patrons] WHERE UserID = @User;", userParam).ToList();
                ViewBag.user = user[0];
            }

            ViewBag.PatronOrUser = PatronOrUser;

            return View();
        }
        public IActionResult Account_Create()
        {
            if (CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");



            return View();
        }

        public IActionResult Account_Create_Action(IFormCollection input)
        {
            if (CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");

            SqlParameter emailParam = new SqlParameter("@Email", SqlDbType.NVarChar);
            emailParam.Value = Convert.ToString(input["email"]);

            if (context.Patrons.Any(param => param.Email == emailParam.Value)) return RedirectToAction("Account_Create"); // No existing emails

            SqlParameter nameParam = new SqlParameter("@Name", SqlDbType.NVarChar);
            nameParam.Value = Convert.ToString(input["name"]);

            SqlParameter passParam = new SqlParameter("@Pass", SqlDbType.VarChar);
            passParam.Value = Convert.ToString(input["password"]);

            string query = "INSERT INTO [Patrons] (Email, Name, Salt, HashPW) " +
                            "VALUES (@Email, @Name, 'Not Implemented', @Pass);";

            context.Database.ExecuteSqlRaw(query, emailParam, nameParam, passParam);

            return Redirect("~/");
        }
        public IActionResult Login()
        {
            if (CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");
            return View();
        }

        public IActionResult Sign_Out() 
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");

            string tokenID = Request.Cookies["TokenCookie"];

            context.Database.ExecuteSqlRaw("DELETE FROM Tokens " + "WHERE TokenId = '" + tokenID + "';");
            
            Response.Cookies.Append("TokenCookie", "0", new CookieOptions
            {
                Expires = DateTime.Now.AddDays(-1)
            });
            return RedirectToAction("Login");
        }

        public IActionResult Login_Action(string username, string password)
        {
            var patron = context.Patrons.FromSqlRaw("SELECT * FROM [Patrons] WHERE Email = '" + username + "';").ToList(); // Patrons have no username, they use their email as user
            bool userOrPatron = false; // False == patron

            int userID = 0;

            if (patron.Count < 1)
            {
                userOrPatron = true; // True == user
                var user = context.Users.FromSqlRaw("SELECT * FROM [User] WHERE UserName = '" + username + "';").ToList();
                if (user.Count < 1)
                {
                    user = context.Users.FromSqlRaw("SELECT * FROM [User] WHERE Email = " + username + "';").ToList();// They can use email as well
                    if (user.Count < 1)
                    {
                        return RedirectToAction("Login");
                    }
                }
                // It's a user
                userID = user[0].UserId;
            } else // It's a patron
            {
                userID = patron[0].UserId;
            }

            SqlParameter userParam = new SqlParameter("@User", SqlDbType.VarChar);
            userParam.Value = userID;

            SqlParameter boolParam = new SqlParameter("@Bool", SqlDbType.Bit);
            boolParam.Value = userOrPatron;

            SqlParameter tokenParam = new SqlParameter("@Token", SqlDbType.Int);

            RandomNumberGenerator rnd = RandomNumberGenerator.Create();
            byte[] bytes = new byte[4]; // 4 bytes for a 32-bit integer
            rnd.GetBytes(bytes);
            int randomNumber = BitConverter.ToInt32(bytes, 0);
            if (randomNumber < 0)
            {
                randomNumber = -randomNumber;
            }

            tokenParam.Value = randomNumber;// Need to check if it already exists

            DateTime issueDate = DateTime.Now;
            SqlParameter issueParam = new SqlParameter("@Issue", SqlDbType.Date);
            issueParam.Value = issueDate;
            
            DateTime expiryDate = issueDate.AddDays(1);
            SqlParameter expiryParam = new SqlParameter("@Expiry", SqlDbType.Date);
            expiryParam.Value = expiryDate;

            string query = "INSERT INTO Tokens (TokenId, UserId, UserOrPatron, IssueDate, ExpiryDate) " +
                           "VALUES (@Token, @User, @Bool, @Issue, @Expiry);";

            context.Database.ExecuteSqlRaw(query, tokenParam, userParam, boolParam, issueParam, expiryParam);

            Response.Cookies.Append("TokenCookie", randomNumber.ToString(), new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = expiryDate
            });

            
            
            return RedirectToAction("Account");
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
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");

            string query =  "SELECT Orders.OrderID, Orders.customer, Orders.StreetAddress, Orders.PostCode, Orders.Suburb, Orders.State " +
                            "FROM [Orders] " +
                            "INNER JOIN [TO] " +
                            "ON Orders.customer = [TO].customerID " +
                            "WHERE [TO].PatronId = @ID;";

            string token = Request.Cookies["TokenCookie"];

            SqlParameter userParam = new SqlParameter("@ID", SqlDbType.Int);
            userParam.Value = CookieConfirm.GetUserID(token);

            var orders = context.Orders.FromSqlRaw(query, userParam).ToList();

            ViewBag.orders = orders;

            return View();
        }

        public IActionResult Update_Order_Details()
        {
            if (!CookieConfirm.IsValidCookie(this.HttpContext, context)) return Redirect("~/");

            
            return View();
        }
    }
}
