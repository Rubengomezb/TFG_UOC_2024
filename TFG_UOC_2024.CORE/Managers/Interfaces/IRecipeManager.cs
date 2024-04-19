using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Models;

namespace TFG_UOC_2024.CORE.Managers.Interfaces
{
    public interface IRecipeManager
    {
        Task<GenericResponse> AddFavorite(RecipeFavorite recipeFavorite);

        Task<GenericResponse> RemoveFavorite(RecipeFavorite recipeFavorite);

        Task<ServiceResponse<RecipeDTO>> GetRecipe(Guid recipeId);

        Task<ServiceResponse<List<CategoryDTO>>> GetCategories();

        Task<ServiceResponse<List<IngredientCategoryDTO>>> GetIngredientsByCategory(Guid categoryId);

        Task<ServiceResponse<List<RecipeDTO>>> GetRecipes();

        Task<GenericResponse> AddIngredients();
    }
}
