using System;
using ExpenseApp.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseApp.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    private const string TableName = "transactions";

    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(transaction => transaction.Id);

        builder.Property(transaction => transaction.Type)
               .HasMaxLength(350)
               .IsRequired();

        builder.Property(transaction => transaction.Amount)
               .HasPrecision(18, 2);

        builder.Property(transaction => transaction.Category)
               .HasMaxLength(350)
               .IsRequired();

        builder.Property(transaction => transaction.Description)
               .HasMaxLength(500)
               .IsRequired();

        builder.HasIndex(transaction => transaction.Amount);
        builder.HasIndex(transaction => transaction.Category);
        builder.HasIndex(transaction => transaction.Description);
    }
}
