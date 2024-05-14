using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Configuration;
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

        Task<List<RecipeDTO>> GetFavouritesAsync(Guid userId);

        Task<RecipeDTO> GetRecipeByIdAsync(Guid recipeId);

        Task<List<RecipeDTO>> GetRecipeByIngredientsAsync(List<string> ingredients, int from, int to);

        Task<List<CategoryDTO>> GetCategories();

        Task<List<IngredientDTO>> GetIngredients(string categoryId);
    }

    public class RecipeService : IRecipeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient httpClient;

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

            httpClient = await GetAuthenticatedHttpClientAsync();
            var response = await httpClient.PostAsJsonAsync("api/Recipe/addFavorite", favourite);

            var content = await response.Content.ReadAsStringAsync();
            bool authResponse =
                JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return authResponse;
        }

        public async Task<bool> DeleteFavourite(Guid recipeId, Guid userId)
        {
            var favourite = new RecipeFavorite
            {
                RecipeId = recipeId,
                UserId = userId
            };

            var httpClient = await GetAuthenticatedHttpClientAsync();

            var response = await httpClient.PostAsJsonAsync("api/Recipe/removeFavorite", favourite).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync();
            bool authResponse =
                JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return authResponse;
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            var httpClient = await GetAuthenticatedHttpClientAsync();
            var response = await httpClient.GetAsync($"api/Recipe/categories", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync();
            List<CategoryDTO> authResponse =
                JsonSerializer.Deserialize<List<CategoryDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return authResponse;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<IngredientDTO>> GetIngredients(string categoryId)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync();

            try
            {
                var response = await httpClient.GetAsync($"api/Recipe/ingredients/{categoryId}", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                var content = await response.Content.ReadAsStringAsync();
                List<IngredientDTO> authResponse =
                    JsonSerializer.Deserialize<List<IngredientDTO>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return authResponse;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<RecipeDTO> GetRecipeByIdAsync(Guid recipeId)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync();

            var response = await httpClient.GetAsync($"api/Recipe/recipe?recipeId={recipeId}", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync();
            RecipeDTO authResponse =
                JsonSerializer.Deserialize<RecipeDTO>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return authResponse;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<RecipeDTO>> GetRecipeByIngredientsAsync(List<string> ingredients, int from, int to)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync();

            var response = await httpClient.GetAsync($"api/Recipe/recipesByIngredients?ingredients={string.Join(",", ingredients)}&from={from}&to={to}", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            var content = await response.Content.ReadAsStringAsync();
            List<RecipeDTO> authResponse =
                JsonSerializer.Deserialize<List<RecipeDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return authResponse;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<RecipeDTO>> GetRecipesAsync()
        {
            var httpClient = await GetAuthenticatedHttpClientAsync();

            var response = await httpClient.GetAsync("api/Recipe/recipes", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

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
            var httpClient = await GetAuthenticatedHttpClientAsync();

            try
            {
                var response = await httpClient.GetAsync($"api/Recipe/favorite?userId={userId}&recipeId={recipeId}", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

                var content = await response.Content.ReadAsStringAsync();
                bool authResponse =
                    JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return authResponse;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<HttpClient> GetAuthenticatedHttpClientAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var authenticatedUser = App.user;

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authenticatedUser.Token);

            return httpClient;
        }

        public async Task<List<RecipeDTO>> GetFavouritesAsync(Guid userId)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync();

            var response = await httpClient.GetAsync($"api/Recipe/favourites?userId={userId}", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {

                var content = await response.Content.ReadAsStringAsync();
                List<RecipeDTO> authResponse =
                    JsonSerializer.Deserialize<List<RecipeDTO>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return authResponse;
            }
            else
            {
                return new List<RecipeDTO>();
            }
        }
    }
}
