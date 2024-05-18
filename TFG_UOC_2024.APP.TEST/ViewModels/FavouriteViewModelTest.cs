using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;
using NUnit.Framework;

namespace TFG_UOC_2024.APP.TEST.ViewModels
{
    public class FavouriteViewModelTest
    {
        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IRecipeService>();
            var viewModel = new FavouriteViewModel(service.Object);
            Assert.IsNotNull(viewModel);
        }

        [Test]
        public void GetFavouriteRecipes()
        {
            App.user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "prueba",
            };

            var service = new Mock<IRecipeService>();

            var viewModel = new FavouriteViewModel(service.Object);

            var rec = new List<RecipeDTO>();
            rec.Add(new RecipeDTO()
            {
                Id = Guid.NewGuid(),
                Name = "prueba",
                Description = "prueba",
                ImageUrl = "prueba",
                IngredientNames = "prueba",
            });

            service.Setup(x => x.GetFavouritesAsync(It.IsAny<Guid>())).ReturnsAsync(rec);
            viewModel.GetFavouriteRecipes();
            Assert.IsTrue(viewModel.Recipes.Any());
        }
    }
}
