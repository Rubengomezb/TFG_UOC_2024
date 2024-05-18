using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.DB.Models;

namespace TFG_UOC_2024.CORE.Models.DTOs
{
    public class RecipeDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public List<IngredientDTO> Ingredients { get; set; }

        public string IngredientNames { get; set; }

        public double? Calories { get; set; }

        public double? Proteins { get; set; }

        public double? Carbohydrates { get; set; }

        public double? Fats { get; set; }
    }

    public class  RecipeFavorite
    {
        public Guid UserId { get; set; }

        public Guid RecipeId { get; set; }
    }

    public class CategoryDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }
    }

    public class IngredientCategoryDTO
    {
        public Guid Id { get; set; }

        public List<IngredientDTO> Ingredients { get; set; }
    }

    public class IngredientDTO
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public double Quantity { get; set; }

        public string CategoryName { get; set; } 
        
        public CategoryDTO Category { get; set; }
    }
}
