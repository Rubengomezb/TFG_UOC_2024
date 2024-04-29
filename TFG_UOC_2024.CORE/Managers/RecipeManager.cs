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

        public async Task<GenericResponse> AddFavorite(RecipeFavorite recipeFavorite)
        {
            var r = new GenericResponse();

            try
            {
                var favorite = _m.Map<UserFavorite>(recipeFavorite);
                await _recipeService.AddFavorite(favorite);
                return r.Ok();
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<GenericResponse> RemoveFavorite(RecipeFavorite recipeFavorite)
        {
            var r = new GenericResponse();

            try
            {
                var favorite = _m.Map<UserFavorite>(recipeFavorite);
                await _recipeService.RemoveFavorite(favorite);
                return r.Ok();
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

        public async Task<ServiceResponse<List<RecipeDTO>>> GetRecipes()
        {
            var r = new ServiceResponse<List<RecipeDTO>>();
            var result = new List<RecipeDTO>();
            try
            {
                var re = await _recipeService.GetRecipe();
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

        public async Task<GenericResponse> AddIngredients()
        {
            //TODO hacer bucle ingredients con el to y from para ir por las diferentes recetas.
            var r = new ServiceResponse<List<RecipeDTO>>();
            var result = new List<RecipeDTO>();
            var categories = new List<CategoryDTO>();
            var ingredientsToAdd = new List<IngredientDTO>();
            int cont = 0;
            try
            {
                while (cont < 500)
                {
                    var from = cont;
                    var to = cont + 50;
                    var re = await _recipeService.GetRecipesByIngredient(from, to + 50);
                    var ingredientsOnDB = _recipeService.GetIngredients().ToList();
                    categories = _m.Map<List<CategoryDTO>>(_recipeService.GetCategories());
                    if (re == null)
                        return r.NotFound("recipe not found");

                    foreach (var rec in re)
                    {
                        var recipes = rec.hits.Select(x => x.recipe);
                        foreach (var recipe in recipes)
                        {
                            var ingredientsDto = _m.Map<List<IngredientDTO>>(recipe.ingredients);

                            foreach (var item in ingredientsDto)
                            {
                                if (!categories.Any(x => x.Name == item.Name))
                                {
                                    var existingCat = categories.FirstOrDefault(x => x.Name == item.Name);
                                    if (existingCat == null)
                                    {
                                        var cat = new CategoryDTO()
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            Name = item.CategoryName,
                                            ImageUrl = item.ImageUrl,
                                        };

                                        categories.Add(cat);
                                        item.Category = cat;
                                    }
                                    else
                                    {
                                        item.Category = _m.Map<CategoryDTO>(existingCat);
                                    }
                                }

                                if (!ingredientsOnDB.Any(x => x.Name.Equals(item.Name)) && !ingredientsToAdd.Any(x => x.Name.Equals(item.Name)))
                                {
                                    ingredientsToAdd.Add(item);
                                }
                            }
                        }
                    }

                    cont++;
                }

                var ingredient = _m.Map<List<Ingredient>>(ingredientsToAdd);
                await _recipeService.AddIngredients(ingredient);
                await _recipeService.AddCategories(_m.Map<List<Category>>(categories));

                return r.Ok();
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

        public async Task<ServiceResponse<List<IngredientCategoryDTO>>> GetIngredientsByCategory(Guid categoryId)
        {
            var r = new ServiceResponse<List<IngredientCategoryDTO>>();
            try
            {
                var re = _recipeService.GetIngredientsByCategory(categoryId);
                if (!re.Any())
                    return r.NotFound("categories not found");

                var ingredientDto = _m.Map<List<IngredientCategoryDTO>>(re);
                return r.Ok(ingredientDto);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<List<RecipeDTO>>> GetRecipesByIngredient(string ingredients, int from, int to)
        {
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
                    result.Add(recipeDto);
                }

                return r.Ok(result);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }
    }
}
