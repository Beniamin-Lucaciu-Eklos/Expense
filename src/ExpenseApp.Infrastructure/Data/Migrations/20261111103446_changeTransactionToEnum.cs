using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeTransactionToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update transactions set [type] ='Expense'");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(350)",
                oldMaxLength: 350);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "transactions",
                type: "nvarchar(350)",
                maxLength: 350,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
