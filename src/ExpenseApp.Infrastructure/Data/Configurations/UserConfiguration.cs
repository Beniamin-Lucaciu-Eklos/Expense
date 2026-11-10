using System;
using ExpenseApp.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseApp.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private const string TableName = "users";

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(TableName);

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Email)
               .HasMaxLength(330)
               .IsRequired();

        builder.HasIndex(user => user.Email).IsUnique();
    }
}
