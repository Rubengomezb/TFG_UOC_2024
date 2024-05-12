using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Managers.Interfaces;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Services.Interfaces;
using AutoMapper;
using TFG_UOC_2024.DB.Models.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using static TFG_UOC_2024.DB.Components.Enums;
using TFG_UOC_2024.CORE.Services.Menu;
using System.Globalization;
using TFG_UOC_2024.DB.Repository;
using TFG_UOC_2024.CORE.Models.ApiModels;

namespace TFG_UOC_2024.CORE.Managers
{
    public class MenuManager : IMenuManager
    {
        private readonly IMenuService _menuService;
        private readonly IUserService _userService;
        private readonly IRecipeService _recipeService;
        private readonly IMapper _m;

        public MenuManager(IMenuService menuService, IUserService userService, IRecipeService recipeService, IMapper m)
        {
            _menuService = menuService;
            _userService = userService;
            _recipeService = recipeService;
            _m = m;
        }

        public async Task<ServiceResponse<IEnumerable<MenuDTO>>> GetMenu(string startTime, string endTime)
        {
            var start = DateTime.ParseExact(startTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var end = DateTime.ParseExact(endTime, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var r = new ServiceResponse<IEnumerable<MenuDTO>>();
            try
            {
                var user = await _userService.GetSelf();
                var re = _menuService.GetMenu(start,end, user.Data.Id);
                //if (!re.Any())
                //    return r.NotFound("recipe not found");

                var menuDto = _m.Map<IEnumerable<MenuDTO>>(re);
                return r.Ok(menuDto);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        public async Task<ServiceResponse<bool>> CreateMenu(DateTime startTime, DateTime endTime, int? foodType)
        {
            var rand = new Random();
            var r = new ServiceResponse<bool>();
            var menuList = new List<MenuDTO>();
            try
            {
                var foodTypeDescription = string.Empty;
                if (foodType.HasValue)
                {
                    foodTypeDescription = GetRestriction((FoodType)foodType.Value);
                }

                var user = await _userService.GetSelf();

                while(startTime <= endTime)
                {
                    foreach (var eatTime in Enum.GetValues(typeof(EatTime)))
                    {
                        var recipe = new RecipeResponse();
                        if ((EatTime)eatTime == EatTime.Breakfast)
                        {
                            recipe = await _recipeService.GetBreakfastRecipe(foodTypeDescription);
                        }
                        else
                        {
                            recipe = await _recipeService.GetRecipe(foodTypeDescription);
                        }

                        var recipeIndex = rand.Next(recipe.hits.Count());
                        var menuDto = new MenuDTO()
                        {
                            EatTime = (EatTime)eatTime,
                            Date = startTime,
                            userId = user.Data.Id,
                            Recipe = _m.Map<RecipeDTO>(recipe.hits[recipeIndex].recipe)
                        };

                        menuDto.Recipe.IngredientNames = string.Join(";", recipe.hits.FirstOrDefault().recipe.ingredientLines);                   

                        menuList.Add(menuDto);
                    }
                   
                    startTime = startTime.AddDays(1);
                }
                
                var menus = _m.Map<List<DB.Models.Menu>>(menuList);
                foreach (var m in menus)
                {
                    m.Recipe.Menu = m;
                }

                await _menuService.CreateWeeklyMenu(menus);
                return r.Ok(true);
            }
            catch (Exception ex)
            {
                return r.BadRequest(ex.Message);
            }
        }

        private string GetRestriction(FoodType type) => type switch
        {
            FoodType.Vegetarian => "vegetarian",
            FoodType.Vegan => "vegan",
            FoodType.Celiac => "gluten-free",
            FoodType.Mediterranean => "Mediterranean",
            FoodType.Muslim => "pork-free",
            FoodType.Diet => "low-sugar",
            FoodType.Diabetic => "low-sugar",
            _ => ""
        };
    }
}
