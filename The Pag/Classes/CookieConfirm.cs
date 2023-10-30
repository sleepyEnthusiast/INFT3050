using Azure.Core;
using Azure;
using The_Pag.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace The_Pag.Classes
{
    public class CookieConfirm
    {
        private static HttpContext _context;
        private static StoreDbContext _dbContext;
        public static void SetHttpContext(HttpContext context) { _context = context; }

        public static void SetDbContext(StoreDbContext context) { _dbContext = context; }

        public CookieConfirm() 
        { 
        
        }

        public static bool IsValidCookie(HttpContext hctx, StoreDbContext dbctx)// Untested
        {
            SetHttpContext(hctx);
            SetDbContext(dbctx);

            if (_context.Request.Cookies["TokenCookie"] == null) return false;

            if (_context.Request.Cookies.TryGetValue("TokenCookie", out string cookie))
            {
                if (DateTime.TryParse(_context.Request.Cookies["TokenCookie"], out DateTime expirationDate))
                {
                    if (expirationDate <= DateTime.Now)
                    {// The cookie is expired
                        
                        _context.Response.Cookies.Delete("TokenCookie");
                        return false;
                    }
                }
                else
                {
                    var tokens = _dbContext.Tokens.FromSqlRaw("SELECT * FROM Tokens").ToList();
                    
                    foreach (var token in tokens)
                    {
                        if (token.ExpiryDate <= DateTime.Now)
                        {
                            _dbContext.Database.ExecuteSqlRaw("DELETE FROM Tokens " + "WHERE TokenId = '" + token.TokenId + "';"); // Delete expired cookie from database
                            if (token.TokenId.ToString() == cookie)
                            {
                                return false;
                            }
                        }
                        if (token.TokenId.ToString() == cookie)
                        {
                            return true;
                        }
                    }
                    // Cookie was not found, so user has a wrong cookie?
                    _dbContext.Database.ExecuteSqlRaw("DELETE FROM Tokens " + "WHERE TokenId = '" + cookie + "';");
                    _context.Response.Cookies.Append("TokenCookie", "0", new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    });

                }
            }
            else
            {// There is no cookie
                return false;
            }
            return false;
        }
    
        public static int HavePermission()// 0 = Error | 1 = Customer | 2 = Staff | 3 = Admin
        {
            if (_context.Request.Cookies.TryGetValue("TokenCookie", out string cookie))
            {
                var tokens = _dbContext.Tokens.FromSqlRaw("SELECT * FROM Tokens").ToList();

                foreach (var token in tokens)
                {
                    if (token.TokenId.ToString() == cookie)
                    {
                        if (token.UserOrPatron) // User
                        {
                            var user = _dbContext.Users.FromSqlRaw("SELECT * FROM [User] WHERE UserID = " + token.UserId).ToList();
                            if (user != null && user.Count > 0)
                            {
                                if (user[0].IsAdmin == true)
                                {
                                    return 3;
                                }
                                return 2;
                            }
                            return 0;
                        } else // Patron
                        {
                            var patron = _dbContext.Patrons.FromSqlRaw("SELECT * FROM [Patrons] WHERE UserID = " + token.UserId.ToString());
                            if(patron != null)
                            {
                                return 1;
                            }
                            return 0;
                        }
                    }
                }
            }
            else
            {// There is no cookie
                return 0;
            }
            return 0;
        }
    }
}
