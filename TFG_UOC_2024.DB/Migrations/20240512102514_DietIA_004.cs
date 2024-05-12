using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG_UOC_2024.DB.Migrations
{
    /// <inheritdoc />
    public partial class DietIA_004 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FoodType",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodType",
                table: "Contact");
        }
    }
}
