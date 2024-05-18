using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFG_UOC_2024.DB.Migrations
{
    /// <inheritdoc />
    public partial class DietIA_003 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_Recipe_RecipeNavigationId",
                table: "Ingredient");

            migrationBuilder.DropIndex(
                name: "IX_Ingredient_RecipeNavigationId",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "RecipeNavigationId",
                table: "Ingredient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecipeNavigationId",
                table: "Ingredient",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_RecipeNavigationId",
                table: "Ingredient",
                column: "RecipeNavigationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Recipe_RecipeNavigationId",
                table: "Ingredient",
                column: "RecipeNavigationId",
                principalTable: "Recipe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
