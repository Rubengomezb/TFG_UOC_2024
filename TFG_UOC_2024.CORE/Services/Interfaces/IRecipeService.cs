using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.CORE.Models.ApiModels;

namespace TFG_UOC_2024.CORE.Services.Interfaces
{
    public interface IRecipeService
    {
        Task<bool> AddFavorite(UserFavorite recipeFavorite);

        Task<bool> RemoveFavorite(UserFavorite recipeFavorite);

        Task<bool> IsFavourite(UserFavorite recipeFavorite);

        Recipe GetRecipe(Guid recipeId);

        IEnumerable<Category> GetCategories();

        Category GetCategoryByName(string name);

        IEnumerable<Category> GetIngredientsByCategory(Guid categoryId);

        Task<RecipeResponse> GetRecipe();

        Task<List<RecipeResponse>> GetRecipesByIngredient(int from, int to);

        Task<bool> AddIngredients(List<Ingredient> ingredients);

        Task<bool> AddCategory(Category category);

        Task<bool> AddCategories(List<Category> category);

        IEnumerable<Ingredient> GetIngredients();

        Task<RecipeResponse> GetCompleteRecipesByIngredient(List<string> ingredients, int from, int to);

        string GetRandomIngredient();
    }
}
