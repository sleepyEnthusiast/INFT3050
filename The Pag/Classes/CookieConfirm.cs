using Azure.Core;
using Azure;
using The_Pag.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        public static bool IsValidCookie()// Untested
        {
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
                            _dbContext.Database.ExecuteSqlRaw("DELETE FROM Tokens " + "WHERE TokenId = " + token.TokenId.ToString()); // Delete expired cookie from database
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

                    
                }
            }
            else
            {// There is no cookie
                return false;
            }
            return false;
        }
    
        public int HavePermission()// 0 = Error | 1 = Customer | 2 = Staff | 3 = Admin
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
                            var user = _dbContext.Users.FromSqlRaw("SELECT * FROM User WHERE UserId = " + token.UserId).ToList();
                            if (user != null)
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
                            var patron = _dbContext.Patrons.FromSqlRaw("SELECT * FROM Patrons WHERE UserID = " + token.UserId.ToString());
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
