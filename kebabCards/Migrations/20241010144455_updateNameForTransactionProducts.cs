using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kebabCards.Migrations
{
    /// <inheritdoc />
    public partial class updateNameForTransactionProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Transactions",
                newName: "ProductNames");

            migrationBuilder.RenameColumn(
                name: "ActionType",
                table: "Transactions",
                newName: "TransactionType");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "Transactions",
                newName: "ActionType");

            migrationBuilder.RenameColumn(
                name: "ProductNames",
                table: "Transactions",
                newName: "ProductName");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
