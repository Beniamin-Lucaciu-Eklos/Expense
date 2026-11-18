using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdForTransaction2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                table: "transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                table: "transactions",
                columns: new[] { "Id", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                table: "transactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                table: "transactions",
                column: "Id");
        }
    }
}
