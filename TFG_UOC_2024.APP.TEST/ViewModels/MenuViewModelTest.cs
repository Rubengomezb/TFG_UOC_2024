using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.TEST.ViewModels
{
    public class MenuViewModelTest
    {
        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IMenuService>();
            var viewModel = new MenuViewModel(service.Object);
            Assert.IsNotNull(viewModel);
        }

        [Test]
        public void InitializeAppointmentsTest()
        {
            var service = new Mock<IMenuService>();

            var viewModel = new MenuViewModel(service.Object);

            var menu = new List<MenuDTO>();
            menu.Add(new MenuDTO()
            {
                Id = Guid.NewGuid(),
                Recipe = new RecipeDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "prueba",
                    Description = "prueba",
                    ImageUrl = "prueba",
                    IngredientNames = "prueba",
                },
                Date = DateTime.Now,
                EatTime = DB.Components.Enums.EatTime.Breakfast,
                userId = Guid.NewGuid(),
            });

            service.Setup(x => x.GetWeeklyMenuAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(menu);
            viewModel.IntializeAppoitments();
            Assert.IsTrue(viewModel.Events.Any());
        }

        [Test]
        public void GetSelectedDateAppointments()
        {
            var service = new Mock<IMenuService>();

            var viewModel = new MenuViewModel(service.Object);

            var menu = new List<MenuDTO>();
            menu.Add(new MenuDTO()
            {
                Id = Guid.NewGuid(),
                Recipe = new RecipeDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "prueba",
                    Description = "prueba",
                    ImageUrl = "prueba",
                    IngredientNames = "prueba",
                },
                Date = DateTime.Now,
                EatTime = DB.Components.Enums.EatTime.Breakfast,
                userId = Guid.NewGuid(),
            });

            service.Setup(x => x.GetWeeklyMenuAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(menu);
            viewModel.IntializeAppoitments();

            viewModel.SelectedDate = DateTime.Now.AddDays(1);
            var result = viewModel.GetSelectedDateAppointments(viewModel.SelectedDate);
            Assert.IsTrue(result.Any());
        }

        [Test]
        public void AddMenuTest()
        {
            var service = new Mock<IMenuService>();

            var viewModel = new MenuViewModel(service.Object);
            var menuList = new List<MenuDTO>();
            var menu = new MenuDTO()
            {
                Id = Guid.NewGuid(),
                Recipe = new RecipeDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "prueba",
                    Description = "prueba",
                    ImageUrl = "prueba",
                    IngredientNames = "prueba",
                },
                Date = DateTime.Now,
                EatTime = DB.Components.Enums.EatTime.Breakfast,
                userId = Guid.NewGuid(),
            };

            menuList.Add(menu);
            service.Setup(x => x.CreateWeeklyMenuAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(true);
            service.Setup(x => x.GetWeeklyMenuAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(menuList);
            viewModel.SelectedDate = DateTime.Now;

            viewModel.AddMenu(viewModel.SelectedDate);
            Assert.IsTrue(viewModel.Events.Any());
        }

        [Test]
        public void ParseMenuResponseTest()
        {
            var service = new Mock<IMenuService>();

            var viewModel = new MenuViewModel(service.Object);

            var menu = new List<MenuDTO>();
            menu.Add(new MenuDTO()
            {
                Id = Guid.NewGuid(),
                Recipe = new RecipeDTO()
                {
                    Id = Guid.NewGuid(),
                    Name = "prueba",
                    Description = "prueba",
                    ImageUrl = "prueba",
                    IngredientNames = "prueba",
                },
                Date = DateTime.Now,
                EatTime = DB.Components.Enums.EatTime.Breakfast,
                userId = Guid.NewGuid(),
            });

            var result = viewModel.ParseMenuResponse(menu.FirstOrDefault());
            Assert.That(menu.FirstOrDefault().Recipe.Name, Is.EqualTo(result.Name));
        }
    }
}
