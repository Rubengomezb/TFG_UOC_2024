using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.TEST.ViewModels
{
    public class RecipeDetailViewModelTest
    {
        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IRecipeService>();
            var viewModel = new RecipeDetailViewModel(service.Object);
            Assert.IsNotNull(viewModel);
        }
        [Test]
        public void GetRecipeDetail()
        {
            var recipeId = Guid.NewGuid();
            App.user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "prueba",
            };

            var service = new Mock<IRecipeService>();

            var viewModel = new RecipeDetailViewModel(service.Object);

            var rec = new RecipeDTO()
            {
                Id = recipeId,
                Name = "prueba",
                Description = "prueba",
                ImageUrl = "prueba",
                IngredientNames = "prueba",
            };
            viewModel.RecipeId = recipeId.ToString();

            service.Setup(x => x.GetRecipeByIdAsync(It.IsAny<Guid>())).ReturnsAsync(rec);
            service.Setup(x => x.IsFavourite(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            viewModel.LoadRecipeDetail();
            Assert.IsTrue(viewModel.Name == "prueba");
        }

        [Test]
        public void MakeFavouriteTest()
        {
            App.user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "prueba",
            };
            var recipeId = Guid.NewGuid();
            var service = new Mock<IRecipeService>();
            var viewModel = new RecipeDetailViewModel(service.Object);
            viewModel.IsFavourite = false;
            viewModel.RecipeId = recipeId.ToString();
            service.Setup(x => x.AddFavourite(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            viewModel.MakeFavourite(true);
            service.Verify(x => x.AddFavourite(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            Assert.IsTrue(viewModel.IsFavourite);
        }

        [Test]
        public void UndoMakeFavouriteTest()
        {
            App.user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "prueba",
            };
            var recipeId = Guid.NewGuid();
            var service = new Mock<IRecipeService>();
            var viewModel = new RecipeDetailViewModel(service.Object);
            viewModel.IsFavourite = true;
            viewModel.RecipeId = recipeId.ToString();
            service.Setup(x => x.DeleteFavourite(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(true);

            viewModel.MakeFavourite(true);

            service.Verify(x => x.DeleteFavourite(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
            Assert.IsFalse(viewModel.IsFavourite);
        }
    }
}
