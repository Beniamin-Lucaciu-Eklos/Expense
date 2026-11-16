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

        builder.Property(user => user.Password)
               .IsRequired();

        builder.Property(user => user.Token)
               .HasMaxLength(2048)
               .HasColumnType("VARCHAR(2048)")
               .IsRequired();

        builder.Property(user => user.Email)
               .HasMaxLength(330)
               .IsRequired();


        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasIndex(user => user.Password).IsUnique();
    }
}
