using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CoverTransactionSummaryIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_UserId_CreatedAt_Id",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserId_CreatedAt_Id",
                table: "transactions",
                columns: new[] { "UserId", "CreatedAt", "Id" })
                .Annotation("SqlServer:Include", new[] { "Amount", "Type" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_UserId_CreatedAt_Id",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserId_CreatedAt_Id",
                table: "transactions",
                columns: new[] { "UserId", "CreatedAt", "Id" });
        }
    }
}
