using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.TEST.ViewModels
{
    public class SearchRecipesViewModelTest
    {
        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IRecipeService>();
            var viewModel = new SearchRecipesViewModel(service.Object);
            Assert.IsNotNull(viewModel);
        }

        [Test]
        public async Task GetRecipesTest()
        {
            var service = new Mock<IRecipeService>();

            var viewModel = new SearchRecipesViewModel(service.Object);

            var rec = new List<RecipeDTO>();
            rec.Add(new RecipeDTO()
            {
                Id = Guid.NewGuid(),
                Name = "prueba",
                Description = "prueba",
                ImageUrl = "prueba",
                IngredientNames = "prueba",
            });

            service.Setup(x => x.GetRecipeByIngredientsAsync(It.IsAny<List<string>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(rec);

            var result = viewModel.GetRecipes().Result;
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void LoadRecipesTest()
        {
            App.user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "prueba",
            };

            var service = new Mock<IRecipeService>();

            var viewModel = new SearchRecipesViewModel(service.Object);

            var rec = new List<RecipeDTO>();
            rec.Add(new RecipeDTO()
            {
                Id = Guid.NewGuid(),
                Name = "prueba",
                Description = "prueba",
                ImageUrl = "prueba",
                IngredientNames = "prueba",
            });

            service.Setup(x => x.GetRecipesAsync()).ReturnsAsync(rec);
            service.Setup(x => x.GetRecipeByIngredientsAsync(It.IsAny<List<string>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(rec);

            viewModel.LoadRecipes();
            Assert.IsTrue(viewModel.Recipes.Any());
        }

        [Test]
        public void OnGenerateAlternatives()
        {
            App.user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "prueba",
            };

            var service = new Mock<IRecipeService>();

            var viewModel = new SearchRecipesViewModel(service.Object);

            var recipeList = new List<RecipeDTO>();
            var rec = new RecipeDTO()
            {
                Id = Guid.NewGuid(),
                Name = "prueba",
                Description = "prueba",
                ImageUrl = "prueba",
                IngredientNames = "prueba",
            };

            recipeList.Add(rec);

            service.Setup(x => x.GetRecipeByIngredientsAsync(It.IsAny<List<string>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(recipeList);
            viewModel.OnGenerateAlternatives(rec);
            Assert.IsTrue(viewModel.Recipes.Any());
            Assert.That(viewModel.from, Is.EqualTo(5));
            Assert.That(viewModel.to, Is.EqualTo(10));
        }
    }
}
