using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.DB.Models;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;

namespace TFG_UOC_2024.APP.Services
{
    public interface IRecipeService
    {
        Task<List<RecipeDTO>> GetRecipesAsync();
        Task<bool> AddFavourite(Guid recipeId, Guid userId);

        Task<bool> DeleteFavourite(Guid recipeId, Guid userId);

        Task<bool> IsFavourite(Guid recipeId, Guid userId);

        Task<RecipeDTO> GetRecipeByIdAsync(Guid recipeId);

        Task<List<RecipeDTO>> GetRecipeByIngredientsAsync(List<string> ingredients, int from, int to);

        Task<List<CategoryDTO>> GetCategories();

        Task<List<IngredientDTO>> GetIngredients(string categoryId);
    }

    public class RecipeService : IRecipeService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RecipeService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> AddFavourite(Guid recipeId, Guid userId)
        {
            var favourite = new RecipeFavorite
            {
                RecipeId = recipeId,
                UserId = userId
            };

            var httpClient = _httpClientFactory.CreateClient("Client");
            var response = await httpClient.PostAsJsonAsync("api/Recipe/addFavorite", favourite);

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<bool> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<bool>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return authResponse.Data;
        }

        public async Task<bool> DeleteFavourite(Guid recipeId, Guid userId)
        {
            var favourite = new RecipeFavorite
            {
                RecipeId = recipeId,
                UserId = userId
            };

            var httpClient = _httpClientFactory.CreateClient("Client");
            var response = await httpClient.PostAsJsonAsync("api/Recipe/removeFavorite", favourite);

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<bool> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<bool>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return authResponse.Data;
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.GetAsync($"api/recipe/categories");

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<List<CategoryDTO>> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<List<CategoryDTO>>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (authResponse.Status == DB.Components.Enums.ServiceStatus.Ok)
            {
                return authResponse.Data;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<IngredientDTO>> GetIngredients(string categoryId)
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.GetAsync($"api/recipe/ingredients/{categoryId}");

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<List<IngredientDTO>> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<List<IngredientDTO>>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (authResponse.Status == DB.Components.Enums.ServiceStatus.Ok)
            {
                return authResponse.Data;
            }
            else
            {
                return null;
            }
        }

        public async Task<RecipeDTO> GetRecipeByIdAsync(Guid recipeId)
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.GetAsync($"api/Recipe/recipe?recipeId={recipeId}");

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<RecipeDTO> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<RecipeDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (authResponse.Status == DB.Components.Enums.ServiceStatus.Ok)
            {
                return authResponse.Data;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<RecipeDTO>> GetRecipeByIngredientsAsync(List<string> ingredients, int from, int to)
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.GetAsync($"api/Recipe/recipe?recipesByIngredients={string.Join(",", ingredients)}&from={from}&to={to}");

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<List<RecipeDTO>> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<List<RecipeDTO>>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (authResponse.Status == DB.Components.Enums.ServiceStatus.Ok)
            {
                return authResponse.Data;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<RecipeDTO>> GetRecipesAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.GetAsync("api/Recipe/recipes");

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<List<RecipeDTO>> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<List<RecipeDTO>>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (authResponse.Status == DB.Components.Enums.ServiceStatus.Ok)
            {
                return authResponse.Data;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsFavourite(Guid recipeId, Guid userId)
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.GetAsync($"api/Recipe/favorite?userId={userId}&recipeId={recipeId}");

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<bool> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<bool>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return authResponse.Data;
        }
    }
}
