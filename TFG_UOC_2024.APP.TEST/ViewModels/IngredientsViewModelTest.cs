using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.TEST.ViewModels
{
    public class IngredientsViewModelTest
    {
        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IRecipeService>();
            var viewModel = new IngredientsViewModel(service.Object);
            Assert.IsNotNull(viewModel);
        }

        [Test]
        public void GetIngredients()
        {
            var service = new Mock<IRecipeService>();

            var viewModel = new IngredientsViewModel(service.Object);

            var ing = new List<IngredientDTO>();
            ing.Add(new IngredientDTO()
            {
                Name = "prueba"
            });

            service.Setup(x => x.GetIngredients(It.IsAny<string>())).ReturnsAsync(ing);
            viewModel.LoadIngredients();
            Assert.IsTrue(viewModel.Ingredients.Any());
        }
    }
}
