using System;
using System.Collections.Generic;

namespace The_Pag;

public partial class Token
{
    public int TokenId { get; set; }

    public int UserId { get; set; }

    public bool UserOrPatron { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime ExpiryDate { get; set; }
}
