using System;

namespace ExpenseApp.Domain.Data.Models;

public class Transaction : BaseEntity
{
    public required string Type { get; set; }

    public double Amount { get; set; }

    public required string Category { get; set; }

    public required string Description { get; set; }

}
