using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TFG_UOC_2024.CORE.Clients;
using TFG_UOC_2024.CORE.Models.ApiModels;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Services.Base;
using TFG_UOC_2024.CORE.Services.Interfaces;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.CORE.Services.Menu
{
    public class RecipeService : BaseService, IRecipeService
    {
        private readonly IUserFavoriteRepository _userFavoriteRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IHttpRecipeClient _httpRecipeClient;

        public string[] ingredientList { get; set; } = new string[] { "chicken", "rice", "pasta", "tomato", "lettuce", "pork", "beef", "pepper", "cheese", "milk", "potato", "spinach" };
        public Dictionary<int, string> ingredientsDict { get; set; } = new Dictionary<int, string>();

        public RecipeService(
            UserManager<ApplicationUser> u,
            IMapper m,
            IHttpContextAccessor hca,
            IRecipeRepository recipeRepository,
            IUserFavoriteRepository userFavoriteRepository,
            ICategoryRepository categoryRepository,
            IIngredientRepository ingredientRepository,
            IHttpRecipeClient httpRecipeClient,
            ILogger logger = null, 
            IConfiguration configuration = null
            ) : base(u, m, hca, logger, configuration)
        {
            _recipeRepository = recipeRepository;
            _userFavoriteRepository = userFavoriteRepository;
            _categoryRepository = categoryRepository;
            _ingredientRepository = ingredientRepository;
            _httpRecipeClient = httpRecipeClient;
            this.SetIngredientList();
        }

        public async Task<bool> AddFavorite(UserFavorite recipeFavorite)
        {
            _userFavoriteRepository.Create(recipeFavorite);
            return true;
        }

        public async Task<bool> RemoveFavorite(UserFavorite recipeFavorite)
        {
            _userFavoriteRepository.Delete(recipeFavorite);
            return true;
        }

        public Recipe GetRecipe(Guid recipeId)
        {
            return _recipeRepository.GetById(recipeId);
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categoryRepository.GetAll();
        }

        public IEnumerable<Category> GetIngredientsByCategory(Guid categoryId)
        {
            return _categoryRepository.GetIngredientByCategoryId(categoryId);
        }

        public async Task<RecipeResponse> GetRecipe()
        {
            return await _httpRecipeClient.GetRecipe(this.GetRandomIngredient());
        }

        public async Task<List<RecipeResponse>> GetRecipesByIngredient(int from, int to)
        {
            var result = new List<RecipeResponse>();
            var ingredientList = new string[] { "chicken", "rice", "pasta", "tomato","lettuce","pork","beef","pepper","cheese","milk","potato","spinach" };

            foreach (var ingredient in ingredientList)
            {
                result.Add(await _httpRecipeClient.GetRecipePaginated(ingredient, from, to));
            }

            return result;
        }

        public async Task<bool> AddIngredients(List<Ingredient> ingredients)
        {
            _ingredientRepository.UpsertRange(ingredients);
            return true;
        }

        public Category GetCategoryByName(string name)
        {
            return _categoryRepository.Find(x => x.Name == name).FirstOrDefault();
        }

        public async Task<bool> AddCategory(Category category)
        {
            _categoryRepository.Create(category);
            return true;
        }

        public async Task<bool> AddCategories(List<Category> category)
        {
            _categoryRepository.UpsertRange(category);
            return true;
        }

        public IEnumerable<Ingredient> GetIngredients()
        {
            return _ingredientRepository.GetAll();
        }

        public void SetIngredientList()
        {
            var ingredientsDict = new Dictionary<int, string>();
            var maxRandom = this.ingredientList.Length -1;

            for (int i = 0; i < maxRandom; i++)
            {
                this.ingredientsDict.Add(i, this.ingredientList[i]);
            }
        }

        public string GetRandomIngredient()
        {
            var maxRandom = this.ingredientList.Length - 1;
            var rnd = new Random();
            var num = rnd.Next(0, maxRandom);
            return this.ingredientsDict.GetValueOrDefault(num);
        }
    }
}
