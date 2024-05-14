using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.TEST.ViewModels
{
    public class Tests
    {
        [Test]
        public void GetCategories()
        {
            var service = new Mock<IRecipeService>();

            var viewModel = new CategoryViewModel(service.Object);

            var cat = new List<CategoryDTO>();
            cat.Add(new CategoryDTO()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "prueba"
            });

            service.Setup(x => x.GetCategories()).ReturnsAsync(cat);
            viewModel.GetCategories();
            Assert.IsTrue(viewModel.Categories.Any());
        }

        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IRecipeService>();
            var viewModel = new CategoryViewModel(service.Object);
            Assert.IsNotNull(viewModel);
        }
    }
}