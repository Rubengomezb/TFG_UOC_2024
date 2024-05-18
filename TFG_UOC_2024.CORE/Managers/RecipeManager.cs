using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TFG_UOC_2024.CORE.Managers.Interfaces;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Services.Interfaces;
using AutoMapper;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.CORE.Models.ApiModels;
using System.Security.Cryptography;
using static System.Net.WebRequestMethods;
using static TFG_UOC_2024.CORE.Components.Authorization.SystemClaims.Permissions;

namespace TFG_UOC_2024.CORE.Managers
{
    public class RecipeManager : IRecipeManager
    {
        private readonly IRecipeService _recipeService;
        private readonly IMapper _m;

        public RecipeManager(IRecipeService recipeService, IMapper m)
        {
            _recipeService = recipeService;
            _m = m;
        }

        public async Task<ServiceResponse<bool>> AddFavorite(RecipeFavorite recipeFavorite)
        {
            var r = new ServiceResponse<bool>();

            try
            {
                var favorite = _m.Map<UserFavorite>(recipeFavorite);
                var added = await _recipeService.AddFavorite(favorite);
                return r.Ok(added);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> RemoveFavorite(RecipeFavorite recipeFavorite)
        {
            var r = new ServiceResponse<bool>();

            try
            {
                var favorite = _recipeService.GetUserFavourite(recipeFavorite.UserId).Result.FirstOrDefault(x => x.RecipeId == recipeFavorite.RecipeId);
                var removed = await _recipeService.RemoveFavorite(favorite);
                return r.Ok(removed);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<RecipeDTO>> GetRecipe(Guid recipeId)
        {
            var r = new ServiceResponse<RecipeDTO>();
            try
            {
                var re = _recipeService.GetRecipe(recipeId);
                if (re == null)
                    return r.NotFound("recipe not found");

                var recipeDto = _m.Map<RecipeDTO>(re);

                return r.Ok(recipeDto);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<RecipeDTO>>> GetRecipes(string health)
        {
            var r = new ServiceResponse<List<RecipeDTO>>();
            var result = new List<RecipeDTO>();
            try
            {
                var re = await _recipeService.GetRecipe(health);
                if (re == null)
                    return r.NotFound("recipe not found");

                var recipes = re.hits.Select(x => x.recipe);
                foreach (var recipe in recipes)
                {
                    var recipeDto = _m.Map<RecipeDTO>(recipe);
                    result.Add(recipeDto);
                }

                return r.Ok(result);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<CategoryDTO>>> GetCategories()
        {
            var r = new ServiceResponse<List<CategoryDTO>>();
            try
            {
                var re = _recipeService.GetCategories();
                if (!re.Any())
                    return r.NotFound("categories not found");

                var recipeDto = _m.Map<List<CategoryDTO>>(re);
                return r.Ok(recipeDto);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> IsFavorite(Guid userId, Guid recipeId)
        {
            var r = new ServiceResponse<bool>();
            var recipefav = new UserFavorite()
            {
                RecipeId = recipeId,
                UserId = userId
            };

            try
            {
                var re = await _recipeService.IsFavourite(recipefav);
                if (re == null || !re)
                    return r.NotFound("categories not found");

                return r.Ok(re);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<IngredientDTO>>> GetIngredientsByCategory(Guid categoryId)
        {
            var r = new ServiceResponse<List<IngredientDTO>>();
            try
            {
                var re = _recipeService.GetIngredientsByCategory(categoryId);
                if (!re.Any())
                    return r.NotFound("categories not found");

                var ingredientDto = new List<IngredientDTO>();
                foreach (var ingredient in re.FirstOrDefault().Ingredients.ToList())
                {
                    ingredientDto.Add(new IngredientDTO()
                    {
                        Name = ingredient.Name,
                        ImageUrl = ingredient.ImageUrl
                    });
                }

                //var ingredientDto = _m.Map<List<IngredientDTO>>(re.FirstOrDefault().Ingredients.ToList());
                return r.Ok(ingredientDto);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<RecipeDTO>>> GetRecipesByIngredient(string ingredients, int from, int to)
        {
            var ingredientsOnDB = _recipeService.GetIngredients().ToList(); 
            var r = new ServiceResponse<List<RecipeDTO>>();
            var result = new List<RecipeDTO>();
            try
            {
                var ingredientList = ingredients.Split(',').ToList();
                var re = await _recipeService.GetCompleteRecipesByIngredient(ingredientList, from, to);
                if (re == null)
                    return r.NotFound("recipe not found");

                var recipes = re.hits.Select(x => x.recipe);
                foreach (var recipe in recipes)
                {
                    var recipeDto = _m.Map<RecipeDTO>(recipe);
                    recipeDto.Id = Guid.NewGuid();
                    var recipeToSave = _m.Map<Recipe>(recipeDto);
                    var recipeResult = _recipeService.GetRecipeByName(recipeToSave.Name);
                    if (recipeResult == null)
                    {
                        recipeDto.Carbohydrates = recipe.digest.FirstOrDefault(x => x.label == "Carbs")?.total;
                        recipeDto.Fats = recipe.digest.FirstOrDefault(x => x.label == "Fat")?.total;
                        recipeDto.Proteins = recipe.digest.FirstOrDefault(x => x.label == "Protein")?.total;
                        recipeDto.IngredientNames = string.Join(";", recipeDto.Ingredients.Select(x => x.Name));
                        recipeToSave.Proteins = recipeDto.Proteins;
                        recipeToSave.Carbohydrates = recipeDto.Carbohydrates;
                        recipeToSave.Fats = recipeDto.Fats;
                        recipeToSave.Calories = recipe.calories;
                        recipeToSave.IngredientNames = recipeDto.IngredientNames;
                        await _recipeService.AddRecipe(recipeToSave);
                    }
                    else
                    {
                        recipeDto = _m.Map<RecipeDTO>(recipeResult);
                    }
                        
                    result.Add(recipeDto);
                }

                return r.Ok(result);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<RecipeDTO>>> GetFavourites(string userId)
        {
            var r = new ServiceResponse<List<RecipeDTO>>();
            try
            {
                var re = await _recipeService.GetFavourite(Guid.Parse(userId));
                if (!re.Any())
                    return r.NotFound("categories not found");

                var recipesDto = new List<RecipeDTO>();
                foreach (var recipe in re.ToList())
                {
                    recipesDto.Add(new RecipeDTO()
                    {
                        Id = recipe.Id,
                        Name = recipe.Name,
                        ImageUrl = recipe.ImageUrl,
                        Description = recipe.Description,
                        IngredientNames = recipe.IngredientNames,
                    });
                }

                return r.Ok(recipesDto);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }
    }
}
