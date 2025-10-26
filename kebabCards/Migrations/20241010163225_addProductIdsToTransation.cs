using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kebabCards.Migrations
{
    /// <inheritdoc />
    public partial class addProductIdsToTransation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductIds",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductIds",
                table: "Transactions");
        }
    }
}
