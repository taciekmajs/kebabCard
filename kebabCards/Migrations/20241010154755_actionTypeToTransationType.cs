using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kebabCards.Migrations
{
    /// <inheritdoc />
    public partial class actionTypeToTransationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActionTime",
                table: "Transactions",
                newName: "TransactionTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionTime",
                table: "Transactions",
                newName: "ActionTime");
        }
    }
}
