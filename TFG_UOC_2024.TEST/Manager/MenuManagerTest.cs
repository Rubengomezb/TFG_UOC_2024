using Microsoft.IdentityModel.Protocols.WSTrust;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Managers;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.ApiModels;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Services.Interfaces;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.TEST.Setting;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.TEST.Manager
{
    public class MenuManagerTest
    {
        [Test]
        public async Task GetMenu_ReturnsMenu()
        {
            //Arrange
            var menuServiceMock = new Mock<IMenuService>();
            var userServiceMock = new Mock<IUserService>();
            var recipeServiceMock = new Mock<IRecipeService>();
            var menuManager = new MenuManager(menuServiceMock.Object, userServiceMock.Object, recipeServiceMock.Object, AutomapperSingleton.Mapper);
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddDays(7);
            var user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = ""
            };

            var menu = new List<DB.Models.Menu>()
            {
                new DB.Models.Menu()
                {
                    Id = Guid.NewGuid(),
                    userId = user.Id,
                }
            };

            var menuDTO = new MenuDTO()
            {
                Id = menu[0].Id,
                userId = user.Id,
                Date = startTime,
            };

            userServiceMock.Setup(x => x.GetSelf()).ReturnsAsync(new ServiceResponse<UserDTO>()
            {
                Data = user
            });

            menuServiceMock.Setup(x => x.GetMenu(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(menu.AsEnumerable());

            //Act
            var result = await menuManager.GetMenu(startTime.ToString("yyyy-MM-dd"), endTime.ToString("yyyy-MM-dd"));

            //Assert
            Assert.That(result.Data.Any(), Is.EqualTo(true));
            Assert.That(result.Data.FirstOrDefault().userId, Is.EqualTo(menuDTO.userId));
            Assert.That(result.Data.FirstOrDefault().Id, Is.EqualTo(menuDTO.Id));
        }

        //add nunit test for Task<GenericResponse> CreateMenu(DateTime startTime, DateTime endTime)
        [Test]
        public async Task CreateMenu_ReturnsGenericResponse()
        {
            //Arrange
            var menuServiceMock = new Mock<IMenuService>();
            var userServiceMock = new Mock<IUserService>();
            var recipeServiceMock = new Mock<IRecipeService>();
            var menuManager = new MenuManager(menuServiceMock.Object, userServiceMock.Object, recipeServiceMock.Object, AutomapperSingleton.Mapper);
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddDays(7);
            var user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = ""
            };

            var recipeResponse = new RecipeResponse()
            {
                hits = new List<Hit>()
                {
                    new Hit()
                    {
                        recipe = new RecipeApi()
                        {
                            label = "recipe",
                            ingredientLines = new List<string>()
                            {
                                "ingredient1",
                                "ingredient2",
                            }.ToArray()
                        }
                    },
                }.ToArray()
            };

            var recipe = new RecipeDTO()
            {
                Name = "recipe",
            };

            var menu = new List<DB.Models.Menu>()
            {
                new DB.Models.Menu()
                {
                    Id = Guid.NewGuid(),
                    userId = user.Id,
                }
            };

            var menuDTO = new MenuDTO()
            {
                Id = menu[0].Id,
                userId = user.Id,
                Date = startTime,
            };

            userServiceMock.Setup(x => x.GetSelf()).ReturnsAsync(new ServiceResponse<UserDTO>()
            {
                Data = user
            });

            recipeServiceMock.Setup(x => x.GetRecipe(It.IsAny<string>())).ReturnsAsync(recipeResponse);
            recipeServiceMock.Setup(x => x.GetBreakfastRecipe(It.IsAny<string>())).ReturnsAsync(recipeResponse);

            menuServiceMock.Setup(x => x.GetMenu(startTime, endTime, user.Id)).Returns(menu);

            //Act
            var result = await menuManager.CreateMenu(startTime, endTime, 0);

            //Assert
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.Ok));
        }

    }
}
