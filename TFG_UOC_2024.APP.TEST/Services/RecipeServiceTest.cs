using Moq;
using NUnit.Framework;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.TEST.Services
{
    public class RecipeServiceTest
    {
        // create nunit test for TFG_UOC_2024.APP.Services.RecipeService constructor
        [Test]
        public void ConstructorTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);

            // Act
            // Assert
            Assert.IsNotNull(viewModel);
        }

        // create nunit test for TFG_UOC_2024.APP.Services.RecipeService.AddFavourite method
        [Test]
        public void AddFavouriteTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);
            var result = viewModel.AddFavourite(Guid.NewGuid(), Guid.NewGuid());

            // Act
            // Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void DeleteFavouriteTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);
            var result = viewModel.DeleteFavourite(Guid.NewGuid(), Guid.NewGuid());

            // Act
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void IsFavouriteTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);
            var result = viewModel.IsFavourite(Guid.NewGuid(), Guid.NewGuid());

            // Act
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetFavouritesAsyncTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);
            var result = viewModel.GetFavouritesAsync(Guid.NewGuid());

            // Act
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetRecipeByIdAsyncTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);
            var result = viewModel.GetRecipeByIdAsync(Guid.NewGuid());

            // Act
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetRecipeByIngredientsAsyncTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);
            var result = viewModel.GetRecipeByIngredientsAsync(new List<string>(), 0, 0);

            // Act
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetCategoriesTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);
            var result = viewModel.GetCategories();

            // Act
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetIngredientsTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);
            var result = viewModel.GetIngredients(Guid.NewGuid().ToString());

            // Act
            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetRecipesAsyncTest()
        {
            // Arrange
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new RecipeService(service.Object);
            var result = viewModel.GetRecipesAsync();

            // Act
            // Assert
            Assert.IsNotNull(result);
        }

        

    }
}
