using System.Data;
using ExpenseApp.Application.Authorization;
using ExpenseApp.Domain.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseApp.Infrastructure.Data;

public sealed class DevelopmentSampleDataSeeder(
    ExpenseDbContext dbContext,
    IPasswordHasher<User> passwordHasher,
    IUserService userService,
    ILogger<DevelopmentSampleDataSeeder> logger)
{
    public const string DemoEmail = "sample-data@expenseapp.local";
    public const string DemoPassword = "SampleData-Only!2026";
    private const string SeedDescriptionPrefix = "Sample dataset #";
    private const int BulkBatchSize = 10_000;

    public async Task SeedAsync(int recordCount, CancellationToken cancellationToken = default)
    {
        if (recordCount is < 1 or > 1_000_000)
        {
            throw new ArgumentOutOfRangeException(
                nameof(recordCount),
                "The sample record count must be between 1 and 1,000,000.");
        }

        await dbContext.Database.MigrateAsync(cancellationToken);

        var demoUser = await dbContext.Users
            .SingleOrDefaultAsync(user => user.Email == DemoEmail, cancellationToken);
        if (demoUser is null)
        {
            var passwordHash = passwordHasher.HashPassword(null!, DemoPassword);
            demoUser = User.Create(DemoEmail, passwordHash);
            demoUser.SetJwtToken(userService.GenerateJwtToken(demoUser));
            dbContext.Users.Add(demoUser);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        var insertedCount = await dbContext.Transactions
            .CountAsync(
                transaction =>
                    transaction.UserId == demoUser.Id &&
                    transaction.Description.StartsWith(SeedDescriptionPrefix),
                cancellationToken);

        if (insertedCount >= recordCount)
        {
            logger.LogInformation(
                "The development dataset already has {RecordCount} or more rows.",
                insertedCount);
            return;
        }

        var sqlConnection = dbContext.Database.GetDbConnection() as SqlConnection
            ?? throw new InvalidOperationException(
                "The development sample seeder requires the SQL Server provider.");

        await dbContext.Database.OpenConnectionAsync(cancellationToken);
        try
        {
            using var bulkCopy = new SqlBulkCopy(
                sqlConnection,
                SqlBulkCopyOptions.TableLock,
                externalTransaction: null)
            {
                DestinationTableName = "dbo.transactions",
                BatchSize = BulkBatchSize,
                BulkCopyTimeout = 0,
                EnableStreaming = true,
            };

            AddColumnMappings(bulkCopy);

            for (var batchStart = insertedCount; batchStart < recordCount; batchStart += BulkBatchSize)
            {
                var batch = CreateBatch(
                    batchStart,
                    Math.Min(BulkBatchSize, recordCount - batchStart),
                    demoUser.Id);
                await bulkCopy.WriteToServerAsync(batch, cancellationToken);
                logger.LogInformation(
                    "Inserted {InsertedCount} of {TargetCount} sample transactions.",
                    batchStart + batch.Rows.Count,
                    recordCount);
            }
        }
        finally
        {
            await dbContext.Database.CloseConnectionAsync();
        }

        logger.LogInformation(
            "Development sample data is ready for {DemoEmail}.",
            DemoEmail);
    }

    private static DataTable CreateBatch(int startIndex, int rowCount, int userId)
    {
        var table = new DataTable();
        table.Columns.Add("Type", typeof(string));
        table.Columns.Add("Amount", typeof(double));
        table.Columns.Add("Category", typeof(string));
        table.Columns.Add("Description", typeof(string));
        table.Columns.Add("CreatedAt", typeof(DateTime));
        table.Columns.Add("UpdatedAt", typeof(DateTime));
        table.Columns.Add("UserId", typeof(int));

        var now = DateTime.UtcNow;
        var currentMonth = new DateTime(
            now.Year,
            now.Month,
            1,
            0,
            0,
            0,
            DateTimeKind.Utc);

        for (var index = startIndex; index < startIndex + rowCount; index++)
        {
            var isIncome = index % 5 == 0;
            var month = currentMonth.AddMonths(-(index % 36));
            var createdAt = month
                .AddDays(index % 27)
                .AddMinutes((index * 37) % (24 * 60));
            var category = isIncome
                ? (index % 2 == 0 ? "Salary" : "Freelance")
                : ((index % 3) switch
                {
                    0 => "Food",
                    1 => "Transportation",
                    _ => "Entertainment",
                });
            var amount = 10 + (index * 7919L % 250_000) / 100d;

            table.Rows.Add(
                isIncome ? "Income" : "Expense",
                amount,
                category,
                $"{SeedDescriptionPrefix}{index + 1:D6}",
                createdAt,
                createdAt,
                userId);
        }

        return table;
    }

    private static void AddColumnMappings(SqlBulkCopy bulkCopy)
    {
        foreach (var columnName in new[]
        {
            "Type",
            "Amount",
            "Category",
            "Description",
            "CreatedAt",
            "UpdatedAt",
            "UserId",
        })
        {
            bulkCopy.ColumnMappings.Add(columnName, columnName);
        }
    }
}
