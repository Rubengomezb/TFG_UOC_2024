using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using TFG_UOC_2024.CORE.Clients;
using TFG_UOC_2024.CORE.Components;
using TFG_UOC_2024.CORE.Models.ApiModels;
using TFG_UOC_2024.DB.Models;

namespace TFG_UOC_2024.TEST.Clients
{
    public class HttpRecipeClientTest
    {
        //add nunit test for Task<RecipeResponse> GetRecipe(string filter)
        [Test]
        public async Task GetRecipeTest()
        {
            //Arrange
            var configForSmsApi = new Dictionary<string, string>
            {
                {"AppSettings:BaseRecipeApiUrl", "https://api.edamam.com/search"},
                {"AppSettings:RecipeApiToken", "a347a5bafab6c8bd4f5d7a1321c24969"},
                {"AppSettings:RecipeApiId", "e83ad64e"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();

            var filter = "chicken";
            var recipeResponse = new RecipeResponse();
            var recipe = new RecipeApi();
            recipeResponse.hits = new List<Hit>().ToArray();
            recipeResponse.hits.Append(new Hit { recipe = recipe });
            var recipeClient = new HttpRecipeClient(configuration);
            //Act
            var result = await recipeClient.GetRecipe(filter, string.Empty);
            //Assert
            Assert.NotNull(result);
            Assert.That(result.hits.Length, Is.EqualTo(50));
        }

        //add nunit test for Task<RecipeResponse> GetRecipePaginated(string filter, int from, int to)
        [Test]
        public async Task GetRecipePaginatedTest()
        {
            //Arrange
            var configForSmsApi = new Dictionary<string, string>
            {
                {"AppSettings:BaseRecipeApiUrl", "https://api.edamam.com/search"},
                {"AppSettings:RecipeApiToken", "a347a5bafab6c8bd4f5d7a1321c24969"},
                {"AppSettings:RecipeApiId", "e83ad64e"},
            };

            var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configForSmsApi)
            .Build();

            var filter = "chicken";
            var from = 0;
            var to = 50;
            var recipeResponse = new RecipeResponse();
            var recipe = new RecipeApi();
            recipeResponse.hits = new List<Hit>().ToArray();
            recipeResponse.hits.Append(new Hit { recipe = recipe });
            var recipeClient = new HttpRecipeClient(configuration);
            //Act
            var result = await recipeClient.GetRecipePaginated(filter, from, to);
            //Assert
            Assert.NotNull(result);
            Assert.That(result.hits.Length, Is.EqualTo(50));
        }
    }
}
