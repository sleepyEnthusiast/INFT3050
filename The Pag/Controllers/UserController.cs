using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using The_Pag.Models;
using System.Security.Cryptography;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using The_Pag.Classes;

namespace The_Pag.Controllers
{
    public class UserController : Controller
    {

        private StoreDbContext context;

        public UserController(StoreDbContext ctx)
        {
            context = ctx;
            CookieConfirm.SetHttpContext(this.HttpContext);
            CookieConfirm.SetDbContext(ctx);
            
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

        public IActionResult Login_Action(string username, string password)
        {
            var patron = context.Patrons.SingleOrDefault(patron => patron.Email == username); // Patrons have no username, they use their email as user
            bool userOrPatron = false; // False == patron

            int userID = 0;

            if (patron == null)
            {
                userOrPatron = true; // True == user
                var user = context.Users.SingleOrDefault(user => user.UserName == username);
                if (user == null)
                {
                    user = context.Users.SingleOrDefault(user => user.Email == username);// They can use email as well
                    if (user == null)
                    {
                        return RedirectToAction("Login");
                    }
                    // It's a user
                    userID = user.UserId;
                }
            } else // It's a patron
            {
                userID = patron.UserId;
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
            return View();
        }

        public IActionResult Update_Order_Details()
        {
            return View();
        }
    }
}
