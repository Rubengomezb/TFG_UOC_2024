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
            //AddIngredients();
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

        public async void AddIngredients()
        {

            var ingredients = new List<IngredientDTO>();
            ingredients.Add(new IngredientDTO() { Name = "Chicken", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/181/original/Fotolia_64510042_XS.jpg?043973", CategoryName = "Meat" });
            ingredients.Add(new IngredientDTO() { Name = "Beef", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/180/original/Fotolia_63808842_XS.jpg?434151", CategoryName = "Meat" });
            ingredients.Add(new IngredientDTO() { Name = "Pork", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/190/original/Fotolia_48282102_XS.jpg?085005", CategoryName = "Meat" });
            ingredients.Add(new IngredientDTO() { Name = "Lettuce", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/695/original/Fotolia_53483671_XS.jpg?896762", CategoryName = "Vegetables" });
            ingredients.Add(new IngredientDTO() { Name = "Mushroom", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/699/original/Fotolia_59358152_XS.jpg?486426", CategoryName = "Vegetables" });
            ingredients.Add(new IngredientDTO() { Name = "Pepper", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/705/original/Fotolia_65015591_XS.jpg?242382", CategoryName = "Vegetables" });
            ingredients.Add(new IngredientDTO() { Name = "Courgette", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/681/original/Fotolia_60420094_XS.jpg?037005", CategoryName = "Vegetables" });
            ingredients.Add(new IngredientDTO() { Name = "Onion", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/700/original/Fotolia_58333191_XS.jpg?071231", CategoryName = "Vegetables" });
            ingredients.Add(new IngredientDTO() { Name = "Potato", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/708/original/Fotolia_36790251_XS.jpg?890535", CategoryName = "Vegetables" });
            ingredients.Add(new IngredientDTO() { Name = "Carrot", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/673/original/Fotolia_62467540_XS.jpg?292378", CategoryName = "Vegetables" });
            ingredients.Add(new IngredientDTO() { Name = "Garlic", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/690/original/Fotolia_59757393_XS.jpg?024210", CategoryName = "Vegetables" });

            ingredients.Add(new IngredientDTO() { Name = "Strawberry", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/713/original/Fotolia_79112171_XS.jpg?985058", CategoryName = "Fruits" });
            ingredients.Add(new IngredientDTO() { Name = "Tangerine", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/715/original/Fotolia_75910504_XS.jpg?435627", CategoryName = "Fruits" });
            ingredients.Add(new IngredientDTO() { Name = "Banana", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/671/original/Fotolia_71934090_XS.jpg?679155", CategoryName = "Fruits" });
            ingredients.Add(new IngredientDTO() { Name = "Peach", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/702/original/Fotolia_40093177_XS.jpg?805521", CategoryName = "Fruits" });

            ingredients.Add(new IngredientDTO() { Name = "Eggs", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/722/original/Fotolia_76475468_XS.jpg?019563", CategoryName = "Eggs" });
            ingredients.Add(new IngredientDTO() { Name = "Cheese", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/145/original/Fotolia_20485702_XS.jpg?087703", CategoryName = "Milky" });
            ingredients.Add(new IngredientDTO() { Name = "Milk", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/278/original/Fotolia_62567939_XS.jpg?965972", CategoryName = "Milky" }); 
            ingredients.Add(new IngredientDTO() { Name = "Pasta", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/206/original/Fotolia_51284648_XS.jpg?979864", CategoryName = "Pasta" });
            ingredients.Add(new IngredientDTO() { Name = "Rice", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/207/original/Fotolia_28347338_XS.jpg?312350", CategoryName = "Pasta" }); 
            ingredients.Add(new IngredientDTO() { Name = "Noodles", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/204/original/Fotolia_58692586_XS.jpg?445971", CategoryName = "Pasta" });
            ingredients.Add(new IngredientDTO() { Name = "Bread", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/198/original/Fotolia_60326169_XS.jpg?147420", CategoryName = "Bread" });

            ingredients.Add(new IngredientDTO() { Name = "Gilthead", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/155/original/Fotolia_69666433_XS.jpg?101919", CategoryName = "Fish" });
            ingredients.Add(new IngredientDTO() { Name = "Cod", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/156/original/Fotolia_65747871_XS.jpg?808299", CategoryName = "Fish" });
            ingredients.Add(new IngredientDTO() { Name = "King prawns", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/160/original/Fotolia_66655874_XS.jpg?394307", CategoryName = "Fish" });
            ingredients.Add(new IngredientDTO() { Name = "Mussels", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/164/original/Fotolia_60054339_XS.jpg?690549", CategoryName = "Fish" });
            ingredients.Add(new IngredientDTO() { Name = "Octopus", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/165/original/Fotolia_70663193_XS.jpg?123617", CategoryName = "Fish" });
            ingredients.Add(new IngredientDTO() { Name = "Salmon", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/169/original/Fotolia_55748744_XS.jpg?667675", CategoryName = "Fish" });
            ingredients.Add(new IngredientDTO() { Name = "Tuna", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/178/original/Fotolia_73217093_XS.jpg?586517", CategoryName = "Fish" });



            var categories = new List<CategoryDTO>();
            categories.Add(new CategoryDTO() { Id = Guid.NewGuid().ToString(), Name = "Vegetables", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/705/original/Fotolia_65015591_XS.jpg?242382" });
            categories.Add(new CategoryDTO() { Id = Guid.NewGuid().ToString(), Name = "Fruits", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/702/original/Fotolia_40093177_XS.jpg?805521" });
            categories.Add(new CategoryDTO() { Id = Guid.NewGuid().ToString(), Name = "Meat", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/180/original/Fotolia_63808842_XS.jpg?434151" });
            categories.Add(new CategoryDTO() { Id = Guid.NewGuid().ToString(), Name = "Bread", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/198/original/Fotolia_60326169_XS.jpg?147420" });
            categories.Add(new CategoryDTO() { Id = Guid.NewGuid().ToString(), Name = "Sweets", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/252/original/Fotolia_77703943_XS.jpg?277703" });
            categories.Add(new CategoryDTO() { Id = Guid.NewGuid().ToString(), Name = "Pasta", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/206/original/Fotolia_51284648_XS.jpg?979864" });
            categories.Add(new CategoryDTO() { Id = Guid.NewGuid().ToString(), Name = "Milky", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/278/original/Fotolia_62567939_XS.jpg?965972" });
            categories.Add(new CategoryDTO() { Id = Guid.NewGuid().ToString(), Name = "Eggs", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/722/original/Fotolia_76475468_XS.jpg?019563" });
            categories.Add(new CategoryDTO() { Id = Guid.NewGuid().ToString(), Name = "Fish", ImageUrl = "https://s3.amazonaws.com/sk.audios.dev/images/156/original/Fotolia_65747871_XS.jpg?808299" });

            var categoriesToAdd = _recipeService.GetCategories();
            //await _recipeService.AddCategories(categoriesToAdd);

            var ingredientsToAdd = _m.Map<List<Ingredient>>(ingredients);
            foreach (var ingredient in ingredientsToAdd)
            {
                var dto = ingredients.FirstOrDefault(x => x.Name == ingredient.Name);
                ingredient.Id = Guid.NewGuid();
                ingredient.Category = categoriesToAdd.Where(x => x.Name == dto.CategoryName).Select(x => x.Id).FirstOrDefault();
            }

            await _recipeService.AddIngredients(ingredientsToAdd);
           
            //TODO hacer bucle ingredients con el to y from para ir por las diferentes recetas.
            /*var r = new ServiceResponse<List<RecipeDTO>>();
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
            }*/
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
                    //recipeToSave.Ingredients = new List<Ingredient>();
                    var recipeResult = _recipeService.GetRecipeByName(recipeToSave.Name);
                    if (recipeResult == null)
                    {
                        recipeDto.IngredientNames = string.Join(";", recipeDto.Ingredients.Select(x => x.Name));
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
