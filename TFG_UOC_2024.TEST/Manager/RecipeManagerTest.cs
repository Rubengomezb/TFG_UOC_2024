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
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.TEST.Setting;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.TEST.Manager
{
    public class RecipeManagerTest
    {
        [Test]
        public async Task AddFavorite_ReturnsTrue()
        {
            // Arrange
            var recipeServiceMock = new Mock<IRecipeService>();
            var recipeManager = new RecipeManager(recipeServiceMock.Object, AutomapperSingleton.Mapper);
            var userFavorite = new UserFavorite()
            {
                Id = Guid.NewGuid(),
                RecipeId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var recipeFavorite = new RecipeFavorite()
            {
                RecipeId = userFavorite.RecipeId,
                UserId = userFavorite.UserId
            };

            recipeServiceMock.Setup(x => x.AddFavorite(userFavorite)).ReturnsAsync(true);

            // Act
            var result = await recipeManager.AddFavorite(recipeFavorite);

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.Ok));
        }

        [Test]
        public async Task RemoveFavorite_ReturnsTrue()
        {
            // Arrange
            var recipeServiceMock = new Mock<IRecipeService>();
            var recipeManager = new RecipeManager(recipeServiceMock.Object, AutomapperSingleton.Mapper);
            var userFavorite = new UserFavorite()
            {
                Id = Guid.NewGuid(),
                RecipeId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            var recipeFavorite = new RecipeFavorite()
            {
                RecipeId = userFavorite.RecipeId,
                UserId = userFavorite.UserId
            };

            recipeServiceMock.Setup(x => x.RemoveFavorite(userFavorite)).ReturnsAsync(true);

            // Act
            var result = await recipeManager.RemoveFavorite(recipeFavorite);

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.Ok));
        }

        [Test]
        public async Task GetRecipe_ReturnsRecipeDTO()
        {
            // Arrange
            var recipeServiceMock = new Mock<IRecipeService>();
            var recipeManager = new RecipeManager(recipeServiceMock.Object, AutomapperSingleton.Mapper);
            var recipe = new Recipe()
            {
                Id = Guid.NewGuid(),
                Name = "Recipe",
                Description = "Description",
            };

            var recipeDTO = new RecipeDTO()
            {
                Name = recipe.Name,
                Description = recipe.Description,
            };

            recipeServiceMock.Setup(x => x.GetRecipe(recipe.Id)).Returns(recipe);

            // Act
            var result = await recipeManager.GetRecipe(recipe.Id);

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.Ok));
            Assert.That(result.Data.Name, Is.EqualTo(recipeDTO.Name));
            Assert.That(result.Data.Description, Is.EqualTo(recipeDTO.Description));
        }

        [Test]
        public async Task GetRecipes_ReturnsListRecipeDTO()
        {
            // Arrange
            var recipeServiceMock = new Mock<IRecipeService>();
            var recipeManager = new RecipeManager(recipeServiceMock.Object, AutomapperSingleton.Mapper);
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

            var recipeDTO = new RecipeDTO()
            {
                Name = recipeResponse.hits.FirstOrDefault().recipe.label,
                Description = "Description",
            };

            recipeServiceMock.Setup(x => x.GetRecipe()).ReturnsAsync(recipeResponse);

            // Act
            var result = await recipeManager.GetRecipes();

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.Ok));
            Assert.That(result.Data.Any(), Is.EqualTo(true));
            Assert.That(result.Data.FirstOrDefault().Name, Is.EqualTo(recipeDTO.Name));
        }

        // Add nunit test for Task<GenericResponse> AddIngredients()
        /*[Test]
        public async Task AddIngredients_ReturnsGenericResponse()
        {
            // Arrange
            var recipeServiceMock = new Mock<IRecipeService>();
            var recipeManager = new RecipeManager(recipeServiceMock.Object, AutomapperSingleton.Mapper);

            recipeServiceMock.Setup(x => x.GetRecipesByIngredient(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new List<RecipeResponse>()
            {
                new RecipeResponse()
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
                }
            });
            recipeServiceMock.Setup(x => x.GetCategories()).Returns(new List<Category>() { new Category() { Id = Guid.NewGuid() } });
            recipeServiceMock.Setup(x => x.GetIngredients()).Returns(new List<Ingredient>() { new Ingredient() { Id = Guid.NewGuid() } });

            // Act
            var result = recipeManager.AddIngredients();

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.Ok));
        }*/

        // Add nunit test for Task<ServiceResponse<List<CategoryDTO>>> GetCategories()
        [Test]
        public async Task GetCategories_ReturnsListCategoryDTO()
        {
            // Arrange
            var recipeServiceMock = new Mock<IRecipeService>();
            var recipeManager = new RecipeManager(recipeServiceMock.Object, AutomapperSingleton.Mapper);
            var category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = "Category",
            };

            var categoryDTO = new CategoryDTO()
            {
                Name = category.Name,
            };

            recipeServiceMock.Setup(x => x.GetCategories()).Returns(new List<Category>() { category });

            // Act
            var result = await recipeManager.GetCategories();

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.Ok));
            Assert.That(result.Data.Any(), Is.EqualTo(true));
            Assert.That(result.Data.FirstOrDefault().Name, Is.EqualTo(categoryDTO.Name));
        }

        [Test]
        public async Task GetIngredientsByCategory_ReturnsListIngredientCategoryDTO()
        {
            // Arrange
            var id = Guid.NewGuid();
            var recipeServiceMock = new Mock<IRecipeService>();
            var recipeManager = new RecipeManager(recipeServiceMock.Object, AutomapperSingleton.Mapper);
            var ingredient = new Ingredient()
            {
                Id = Guid.NewGuid(),
                Name = "Ingredient",
            };

            var ingredientCategoryDTO = new IngredientCategoryDTO()
            {
                Id = id,
            };

            recipeServiceMock.Setup(x => x.GetIngredientsByCategory(It.IsAny<Guid>())).Returns(
                new List<Category>() 
                { new Category()
                    {
                        Id = id,
                    }
                });

            // Act
            var result = await recipeManager.GetIngredientsByCategory(Guid.NewGuid());

            // Assert
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.Ok));
            Assert.That(result.Data.Any(), Is.EqualTo(true));
        }


    }
}
