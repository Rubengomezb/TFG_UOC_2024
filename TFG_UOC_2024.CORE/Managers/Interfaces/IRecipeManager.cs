using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.ApiModels;

namespace TFG_UOC_2024.CORE.Managers.Interfaces
{
    public interface IRecipeManager
    {
        Task<ServiceResponse<bool>> AddFavorite(RecipeFavorite recipeFavorite);

        Task<ServiceResponse<bool>> RemoveFavorite(RecipeFavorite recipeFavorite);

        Task<ServiceResponse<bool>> IsFavorite(Guid userId, Guid recipeId);

        Task<ServiceResponse<List<RecipeDTO>>> GetFavourites(string userId);

        Task<ServiceResponse<RecipeDTO>> GetRecipe(Guid recipeId);

        Task<ServiceResponse<List<RecipeDTO>>> GetRecipesByIngredient(string ingredients, int from, int to);

        Task<ServiceResponse<List<CategoryDTO>>> GetCategories();

        Task<ServiceResponse<List<IngredientDTO>>> GetIngredientsByCategory(Guid categoryId);

        Task<ServiceResponse<List<RecipeDTO>>> GetRecipes(string health);

    }
}
