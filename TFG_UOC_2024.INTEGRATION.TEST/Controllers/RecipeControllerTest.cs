using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.INTEGRATION.TEST.Base;

namespace TFG_UOC_2024.INTEGRATION.TEST.Controllers
{
    public class RecipeControllerTest : TestBase
    {
        private List<Guid> recipes;

        public RecipeControllerTest() 
        {
            recipes = new List<Guid>();
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

        // add integration test for GetIngredientsByCategory
        [Test]
        public async Task GetIngredientsByCategoryNotFound()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/Recipe/ingredients/{Guid.NewGuid()}";
            var response = await client.GetAsync(request);

            Assert.IsTrue(response.StatusCode == HttpStatusCode.NotFound);
        }

        [Test]
        public async Task GetIngredientsByCategory()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var requestCategories = $"api/Recipe/categories";

            var responseCategories = await client.GetAsync(requestCategories);
            responseCategories.EnsureSuccessStatusCode();
            var contentCategories = await responseCategories.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<IEnumerable<CategoryDTO>>(contentCategories, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var request = $"api/Recipe/ingredients/{categories.Select(x => x.Id).FirstOrDefault()}";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var ingredients = JsonSerializer.Deserialize<IEnumerable<IngredientDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(ingredients.Any());
            Assert.That(4, Is.EqualTo(ingredients.Count()));
        }

        [Test]
        public async Task AddAndGetRecipesByIngredient()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/Recipe/recipesByIngredients?ingredients=chicken&from=0&to=10";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var recipes = JsonSerializer.Deserialize<IEnumerable<RecipeDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            this.recipes.AddRange(recipes.Select(x => x.Id));

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(recipes.Any());
        }

        [Test]
        public async Task AddFavourite()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/Recipe/addFavorite";
            var response = await client.PostAsJsonAsync(request, new RecipeFavorite
            {
                UserId = this.UserId,
                RecipeId = this.recipes.FirstOrDefault()
            });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemoveFavorite()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/Recipe/removeFavorite";
            var response = await client.PostAsJsonAsync(request, new RecipeFavorite
            {
                UserId = this.UserId,
                RecipeId = this.recipes.FirstOrDefault()
            });
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(response.StatusCode == HttpStatusCode.OK);
            Assert.IsTrue(result);
        }
    }
}
