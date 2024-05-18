using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TFG_UOC_2024.CORE.Clients;
using TFG_UOC_2024.CORE.Models.ApiModels;
using TFG_UOC_2024.CORE.Services.Interfaces;
using TFG_UOC_2024.CORE.Services.Menu;
using TFG_UOC_2024.CORE.Services.User;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;

namespace TFG_UOC_2024.TEST.Service
{
    public class RecipeServiceTest
    {
        RecipeService _service;
        Mock<IRecipeRepository> _mockRepository;
        Mock<IUserFavoriteRepository> _mockUserFavoriteRepository;
        Mock<ICategoryRepository> _mockCategoryRepository;
        Mock<IIngredientRepository> _mockIngredientRepository;
        Mock<IHttpRecipeClient> _mockRecipeClient;

        [SetUp]
        public void Setup()
        {
            var mockLogger = new Mock<ILogger<UserService>>();
            var mockMapper = new Mock<IMapper>();
            var mockConfig = new Mock<IConfiguration>();
            var store = new Mock<IUserStore<ApplicationUser>>();
            store.Setup(x => x.FindByIdAsync("123", CancellationToken.None))
                .ReturnsAsync(new ApplicationUser()
                {
                    UserName = "test@email.com",
                    Id = Guid.NewGuid(),
                });

            var mockUserManager = new UserManager<ApplicationUser>(store.Object, null, null, null, null, null, null, null, null);
            var mapperMock = new Mock<IMapper>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _mockRepository = new Mock<IRecipeRepository>();
            _mockUserFavoriteRepository = new Mock<IUserFavoriteRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();  
            _mockIngredientRepository = new Mock<IIngredientRepository>();
            _mockRecipeClient = new Mock<IHttpRecipeClient>();
            var loggerMock = new Mock<ILogger>();
            var configurationMock = new Mock<IConfiguration>();

            _service = new RecipeService(
                mockUserManager,
                mapperMock.Object,
                httpContextAccessorMock.Object,
                _mockRepository.Object,
                _mockUserFavoriteRepository.Object,
                _mockCategoryRepository.Object,
                _mockIngredientRepository.Object,
                _mockRecipeClient.Object,
                loggerMock.Object,
                configurationMock.Object
            );
        }

        //add nunit test for Task<bool> AddFavorite(UserFavorite recipeFavorite)
        [Test]
        public async Task AddFavorite_ReturnsTrue()
        {
            // Arrange
            var recipeFavorite = new UserFavorite()
            {
                Id = Guid.NewGuid(),
                RecipeId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _mockUserFavoriteRepository.Setup(x => x.Create(recipeFavorite, true)).Verifiable();

            // Act
            var result = await _service.AddFavorite(recipeFavorite);

            // Assert
            Assert.IsTrue(result);
        }

        //add nunit test for Task<bool> RemoveFavorite(UserFavorite recipeFavorite)
        [Test]
        public async Task RemoveFavorite_ReturnsTrue()
        {
            // Arrange
            var recipeFavorite = new UserFavorite()
            {
                Id = Guid.NewGuid(),
                RecipeId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _mockUserFavoriteRepository.Setup(x => x.Delete(recipeFavorite, true)).Verifiable();

            // Act
            var result = await _service.RemoveFavorite(recipeFavorite);

            // Assert
            Assert.IsTrue(result);
        }

        //add nunit test for Recipe GetRecipe(Guid recipeId)
        [Test]
        public void GetRecipe_ReturnsRecipe()
        {
            // Arrange
            var recipeId = Guid.NewGuid();
            var recipe = new Recipe { Id = recipeId };
            _mockRepository.Setup(m => m.GetById(recipeId)).Returns(recipe);

            // Act
            var result = _service.GetRecipe(recipeId);

            // Assert
            Assert.That(recipe, Is.EqualTo(result));
        }

        //add nunit test for IEnumerable<Category> GetCategories()
        [Test]
        public void GetCategories_ReturnsCategories()
        {
            // Arrange
            var categories = new List<Category> { new Category() };
            _mockCategoryRepository.Setup(m => m.GetAll()).Returns(categories.AsQueryable);

            // Act
            var result = _service.GetCategories();

            // Assert
            Assert.That(categories, Is.EqualTo(result));
        }

        //add nunit test for IEnumerable<Category> GetIngredientsByCategory(Guid categoryId)
        [Test]
        public void GetIngredientsByCategory_ReturnsIngredients()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var ingredients = new List<Category> { new Category() };
            _mockCategoryRepository.Setup(m => m.GetIngredientByCategoryId(categoryId)).Returns(ingredients.AsQueryable);

            // Act
            var result = _service.GetIngredientsByCategory(categoryId);

            // Assert
            Assert.That(ingredients, Is.EqualTo(result));
        }

        //add nunit test for Task<RecipeResponse> GetRecipe()
        [Test]
        public async Task GetRecipe_ReturnsRecipeResponse()
        {
            // Arrange
            var recipeResponse = new RecipeResponse();
            _mockRecipeClient.Setup(x => x.GetRecipe(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(recipeResponse);

            // Act
            var result = await _service.GetRecipe(It.IsAny<string>());

            // Assert
            Assert.That(recipeResponse, Is.EqualTo(result));
        }

        //add nunit test for Task<List<RecipeResponse>> GetRecipesByIngredient(int from, int to)
        [Test]
        public async Task GetRecipesByIngredient_ReturnsRecipeResponseList()
        {
            // Arrange
            var from = 0;
            var to = 10;
            var recipeResponse = new RecipeResponse();
            var recipeResponses = new List<RecipeResponse> { recipeResponse };
            _mockRecipeClient.Setup(x => x.GetRecipePaginated(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(recipeResponse);

            // Act
            var result = await _service.GetRecipesByIngredient(from, to);

            // Assert
            Assert.That(recipeResponses.FirstOrDefault(), Is.EqualTo(result.FirstOrDefault()));
            Assert.That(result.Count, Is.EqualTo(12));
        }

        [Test]
        public async Task AddIngredients_ReturnsTrue()
        {
            // Arrange
            var ingredients = new List<Ingredient> { new Ingredient() };

            _mockIngredientRepository.Setup(x => x.UpsertRange(ingredients)).Verifiable();

            // Act
            var result = await _service.AddIngredients(ingredients);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetCategoryByName_ReturnsCategory()
        {
            // Arrange
            var name = "category";
            var category = new Category { Name = name };
            _mockCategoryRepository.Setup(m => m.Find(x => x.Name == name)).Returns(new List<Category> { category }.AsQueryable);

            // Act
            var result = _service.GetCategoryByName(name);

            // Assert
            Assert.That(category, Is.EqualTo(result));
        }

        //add nunit test for Task<bool> AddCategory(Category category)
        [Test]
        public async Task AddCategory_ReturnsTrue()
        {
            // Arrange
            var category = new Category { Name = "category" };

            _mockCategoryRepository.Setup(x => x.Create(category, true)).Verifiable();

            // Act
            var result = await _service.AddCategory(category);

            // Assert
            Assert.IsTrue(result);
        }

        //add nunit test for Task<bool> AddCategories(List<Category> category)
        [Test]
        public async Task AddCategories_ReturnsTrue()
        {
            // Arrange
            var categories = new List<Category> { new Category() };

            _mockCategoryRepository.Setup(x => x.UpsertRange(categories)).Verifiable();

            // Act
            var result = await _service.AddCategories(categories);

            // Assert
            Assert.IsTrue(result);
        }

        //add nunit test for IEnumerable<Ingredient> GetIngredients()
        [Test]
        public void GetIngredients_ReturnsIngredients()
        {
            // Arrange
            var ingredients = new List<Ingredient> { new Ingredient() };
            _mockIngredientRepository.Setup(m => m.GetAll()).Returns(ingredients.AsQueryable);

            // Act
            var result = _service.GetIngredients();

            // Assert
            Assert.That(ingredients, Is.EqualTo(result));
        }

        //add nunit test for GetRandomIngredient
        [Test]
        public void GetRandomIngredient_ReturnsString()
        {
            // Arrange
            var ingredientList = new string[] { "chicken", "rice", "pasta", "tomato", "lettuce", "pork", "beef", "pepper", "cheese", "milk", "potato", "spinach" };

            // Act
            var result = _service.GetRandomIngredient();

            // Assert
            Assert.That(ingredientList.Contains(result), Is.EqualTo(true));
        }
    }
}
