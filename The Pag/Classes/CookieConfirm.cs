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

        public static bool IsValidCookie()//untested
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
                            _dbContext.Database.ExecuteSqlRaw("DELETE FROM Tokens " + "WHERE TokenId = " + token.TokenId.ToString());
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
    }
}
