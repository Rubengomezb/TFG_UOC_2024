using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TFG_UOC_2024.CORE.Managers;
using TFG_UOC_2024.CORE.Managers.Interfaces;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;

namespace TFG_UOC_2024.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    public class RecipeController : BaseController
    {
        private readonly IRecipeManager _recipeManager;

        #region Constructor
        public RecipeController(UserManager<ApplicationUser> u, ApplicationContext dbContext, IMapper m, IRecipeManager recipeManager, ILogger logger = null, IConfiguration configuration = null) : base(u, dbContext, m, logger, configuration)
        {
            _recipeManager = recipeManager;
        }
        #endregion

        #region Crud
        [HttpGet("recipe")]
        public async Task<ActionResult> GetRecipe(Guid recipeId) =>
            Respond(await _recipeManager.GetRecipe(recipeId));

        [HttpGet("recipes")]
        public async Task<ActionResult> GetRecipes(string health) =>
            Respond(await _recipeManager.GetRecipes(health));

        [HttpGet("recipesByIngredients")]
        public async Task<ActionResult> GetRecipes(string ingredients, int from, int to) =>
            Respond(await _recipeManager.GetRecipesByIngredient(ingredients, from, to));

        [HttpGet("categories")]
        public async Task<ActionResult> GetCategories() =>
            Respond(await _recipeManager.GetCategories());

        [HttpGet("ingredients/{categoryId}")]
        public async Task<ActionResult> GetIngredientsByCategory(string categoryId) =>
            Respond(await _recipeManager.GetIngredientsByCategory(Guid.Parse(categoryId)));

        [HttpPost("addFavorite")]
        public async Task<ActionResult>AddFavority([FromBody] RecipeFavorite recipeFavorite) =>
            Respond(await _recipeManager.AddFavorite(recipeFavorite));

        [HttpGet("favorite")]
        public async Task<ActionResult> IsFavorite(Guid userId, Guid recipeId) =>
            Respond(await _recipeManager.IsFavorite(userId, recipeId));

        [HttpPost("removeFavorite")]
        public async Task<ActionResult> RemoveFavority([FromBody] RecipeFavorite recipeFavorite) =>
            Respond(await _recipeManager.RemoveFavorite(recipeFavorite));

        [HttpGet("favourites")]
        public async Task<ActionResult> GetFavourites(string userId) =>
            Respond(await _recipeManager.GetFavourites(userId));
        #endregion
    }
}