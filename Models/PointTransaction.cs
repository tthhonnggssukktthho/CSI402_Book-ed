using System;
using System.Collections.Generic;

namespace _66014444_Project.Models;

public partial class PointTransaction
{
    public int PointTxnId { get; set; }

    public int CustomerId { get; set; }

    public int? OrderId { get; set; }

    public string TransactionType { get; set; } = null!;

    public int Points { get; set; }

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int? CreatedByUserId { get; set; }

    public virtual User? CreatedByUser { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order? Order { get; set; }
}
