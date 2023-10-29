using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace The_Pag;

public partial class Token
{
    [Key]
    public int TokenId { get; set; }

    public int UserId { get; set; }

    public bool UserOrPatron { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ExpiryDate { get; set; }
}
