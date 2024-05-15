using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG_UOC_2024.DB.Migrations
{
    /// <inheritdoc />
    public partial class DietIA_005 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Calories",
                table: "Recipe",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Carbohydrates",
                table: "Recipe",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Fats",
                table: "Recipe",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Proteins",
                table: "Recipe",
                type: "double",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calories",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "Carbohydrates",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "Fats",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "Proteins",
                table: "Recipe");
        }
    }
}
