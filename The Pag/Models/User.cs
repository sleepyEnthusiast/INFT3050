using System;
using System.Collections.Generic;

namespace The_Pag;

public partial class User
{
    public string UserName { get; set; } = null!;

    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public bool? IsAdmin { get; set; }

    public string? Salt { get; set; }

    public string? HashPw { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
