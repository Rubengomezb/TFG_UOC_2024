using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.API.Controllers;
using TFG_UOC_2024.CORE.Managers.Interfaces;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.TEST.Controller
{
    public  class RecipeControllerTest
    {
        public RecipeController controller;
        public Mock<IRecipeManager> recipeManagerMock;

        [SetUp]
        public void SetUp()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            store.Setup(x => x.FindByIdAsync("123", CancellationToken.None))
                .ReturnsAsync(new ApplicationUser()
                {
                    UserName = "test@email.com",
                    Id = Guid.NewGuid(),
                });

            var mockUserManager = new UserManager<ApplicationUser>(store.Object, null, null, null, null, null, null, null, null);

            var mockDbContext = new Mock<ApplicationContext>();
            var mockMapper = new Mock<IMapper>();
            recipeManagerMock = new Mock<IRecipeManager>();
            var mockLogger = new Mock<ILogger>();
            var mockConfiguration = new Mock<IConfiguration>();

            controller = new RecipeController(mockUserManager, mockDbContext.Object, mockMapper.Object, recipeManagerMock.Object, mockLogger.Object, mockConfiguration.Object);
        }

        [Test]
        public async Task GetRecipeById_ReturnsRecipe()
        {
            var id = Guid.NewGuid();
            var expectedRecipe = new RecipeDTO();
            var serviceResponse = new ServiceResponse<RecipeDTO>()
            {
                Data = expectedRecipe,
            };

            // Asume que Menu es la clase que retorna GetMenu
            recipeManagerMock.Setup(m => m.GetRecipe(id)).ReturnsAsync(serviceResponse);

            // Act
            ActionResult? actionResult = await controller.GetRecipe(id);

            // Assert
            var result = ((OkObjectResult)actionResult).Value as RecipeDTO;
            Assert.IsNotNull(actionResult);
            Assert.That(result, Is.EqualTo(expectedRecipe));
        }

        [Test]
        public async Task GetAllRecipes_ReturnsOk()
        {
            var expectedRecipe = new List<RecipeDTO>()
                {
                    new RecipeDTO(),
                };

            var serviceResponse = new ServiceResponse<List<RecipeDTO>>()
            {
                Data = expectedRecipe
            };

            // Asume que Menu es la clase que retorna GetMenu
            recipeManagerMock.Setup(m => m.GetRecipes()).ReturnsAsync(serviceResponse);

            // Act
            ActionResult? actionResult = await controller.GetRecipes();

            // Assert
            var result = ((OkObjectResult)actionResult).Value as List<RecipeDTO>;
            Assert.IsNotNull(actionResult);
            Assert.That(result, Is.EqualTo(expectedRecipe));
        }

        [Test]
        public async Task GetAllCategories_ReturnsOk()
        {
            var expectedRecipe = new List<CategoryDTO>()
                {
                    new CategoryDTO(),
                };

            var serviceResponse = new ServiceResponse<List<CategoryDTO>>()
            {
                Data = expectedRecipe
            };

            // Asume que Menu es la clase que retorna GetMenu
            recipeManagerMock.Setup(m => m.GetCategories()).ReturnsAsync(serviceResponse);

            // Act
            ActionResult? actionResult = await controller.GetCategories();

            // Assert
            var result = ((OkObjectResult)actionResult).Value as List<CategoryDTO>;
            Assert.IsNotNull(actionResult);
            Assert.That(result, Is.EqualTo(expectedRecipe));
        }

        [Test]
        public async Task GetCategoryById_ReturnsRecipe()
        {
            var id = Guid.NewGuid();
            var expectedRecipe = new List<IngredientDTO>() { new IngredientDTO(), };
            var serviceResponse = new ServiceResponse<List<IngredientDTO>>()
            {
                Data = expectedRecipe,
            };

            // Asume que Menu es la clase que retorna GetMenu
            recipeManagerMock.Setup(m => m.GetIngredientsByCategory(id)).ReturnsAsync(serviceResponse);

            // Act
            ActionResult? actionResult = await controller.GetIngredientsByCategory(id.ToString());

            // Assert
            var result = ((OkObjectResult)actionResult).Value as List<IngredientDTO>; ;
            Assert.IsNotNull(actionResult);
            Assert.That(result, Is.EqualTo(expectedRecipe));
        }

        [Test]
        public async Task AddFavorite_returnsOK()
        {
            var expectedFavorite = new RecipeFavorite()
            {
                RecipeId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            };

            var genericResp = new GenericResponse()
            {
                Status = ServiceStatus.Ok
            };

            // Asume que Menu es la clase que retorna GetMenu
            recipeManagerMock.Setup(m => m.AddFavorite(expectedFavorite)).ReturnsAsync(genericResp);

            // Act
            ActionResult? actionResult = await controller.AddFavority(expectedFavorite);

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkResult)));
            Assert.That(((OkResult)actionResult).StatusCode, Is.EqualTo(200));
        }

        /*[Test]
        public async Task AddIngredients_returnsOK()
        {
            var genericResp = new GenericResponse()
            {
                Status = ServiceStatus.Ok
            };

            // Asume que Menu es la clase que retorna GetMenu
            recipeManagerMock.Setup(m => m.AddIngredients()).ReturnsAsync(genericResp);

            // Act
            ActionResult? actionResult = await controller.AddIngredients();

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkResult)));
            Assert.That(((OkResult)actionResult).StatusCode, Is.EqualTo(200));
        }*/

        [Test]
        public async Task RemoveFavorite_returnsOK()
        {
            var expectedFavorite = new RecipeFavorite()
            {
                RecipeId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            };

            var genericResp = new GenericResponse()
            {
                Status = ServiceStatus.Ok
            };

            // Asume que Menu es la clase que retorna GetMenu
            recipeManagerMock.Setup(m => m.RemoveFavorite(expectedFavorite)).ReturnsAsync(genericResp);

            // Act
            ActionResult? actionResult = await controller.RemoveFavority(expectedFavorite);

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkResult)));
            Assert.That(((OkResult)actionResult).StatusCode, Is.EqualTo(200));
        }
    }
}
