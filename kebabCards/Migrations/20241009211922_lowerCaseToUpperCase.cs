using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace kebabCards.Migrations
{
    /// <inheritdoc />
    public partial class lowerCaseToUpperCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "pointsEarned",
                table: "Products",
                newName: "PointsEarned");

            migrationBuilder.RenameColumn(
                name: "pointsCost",
                table: "Products",
                newName: "PointsCost");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PointsEarned",
                table: "Products",
                newName: "pointsEarned");

            migrationBuilder.RenameColumn(
                name: "PointsCost",
                table: "Products",
                newName: "pointsCost");
        }
    }
}
