using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.INTEGRATION.TEST.Base;

namespace TFG_UOC_2024.INTEGRATION.TEST.Controllers
{
    public class RecipeControllerTest : TestBase
    {
        public RecipeControllerTest() 
        {
        }

        [Test]
        public async Task GetRecipes()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = "api/Recipe/recipes?health=Mediterranean";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var recipe = JsonSerializer.Deserialize<IEnumerable<RecipeDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(recipe != null);
        }

        [Test]
        public async Task GetRecipeById()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/recipe/recipe/{Guid.NewGuid()}";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var recipe = JsonSerializer.Deserialize<RecipeDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(recipe != null);
        }

        [Test]
        public async Task AddIngredientToRecipe()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/recipe/recipe/{Guid.NewGuid()}/ingredient";
            var response = await client.PostAsJsonAsync(request, new { ingredientId = Guid.NewGuid(), quantity = 1 });
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task RemoveIngredientFromRecipe()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/recipe/recipe/{Guid.NewGuid()}/ingredient/{Guid.NewGuid()}";
            var response = await client.DeleteAsync(request);
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetIngredientsFromRecipe()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/recipe/recipe/{Guid.NewGuid()}/ingredient";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var ingredients = JsonSerializer.Deserialize<IEnumerable<IngredientDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(ingredients != null);
        }

        [Test]
        public async Task GetRecipeByIngredient()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/recipe/recipe/ingredient/{Guid.NewGuid()}";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var recipes = JsonSerializer.Deserialize<IEnumerable<RecipeDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(recipes != null);
        }

        [Test]
        public async Task GetRecipeByCategory()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/recipe/recipe/category/{Guid.NewGuid()}";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var recipes = JsonSerializer.Deserialize<IEnumerable<RecipeDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(recipes != null);
        }

        // add integration tests here for GetCategories()
        [Test]
        public async Task GetCategories()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/Recipe/categories";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<IEnumerable<CategoryDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(categories != null);
        }

    }
}
