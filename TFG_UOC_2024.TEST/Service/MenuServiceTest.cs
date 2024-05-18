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
using TFG_UOC_2024.CORE.Managers;
using TFG_UOC_2024.CORE.Services.Menu;
using TFG_UOC_2024.CORE.Services.User;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;
using TFG_UOC_2024.TEST.Setting;

namespace TFG_UOC_2024.TEST.Service
{
    public class MenuServiceTest
    {
        MenuService _service;
        Mock<IMenuRepository> _mockRepository;

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
            _mockRepository = new Mock<IMenuRepository>();
            var loggerMock = new Mock<ILogger>();
            var configurationMock = new Mock<IConfiguration>();

            _service = new MenuService(
                mockUserManager,
                mapperMock.Object,
                httpContextAccessorMock.Object,
                _mockRepository.Object,
                loggerMock.Object,
                configurationMock.Object
            );
        }


        //add nunit test for IEnumerable<DB.Models.Menu> GetMenu(DateTime startTime, DateTime endTime, Guid userId)
        [Test]
        public void GetMenu_ReturnsMenu()
        {
            // Arrange
            var startTime = DateTime.Now;
            var endTime = DateTime.Now.AddDays(7);
            var userId = Guid.NewGuid();
            var menu = new Menu { userId = userId, CreatedOn = startTime };
            var menus = new List<Menu> { menu };
            _mockRepository.Setup(m => m.GetMenu(startTime, endTime, userId)).Returns(menus);

            // Act
            var result = _service.GetMenu(startTime, endTime, userId);

            // Assert
            Assert.That(menus, Is.EqualTo(result));
        }

        [Test]
        public async Task CreateWeeklyMenu_ReturnsTrue()
        {
            // Arrange
            var menus = new List<Menu> { new Menu() };

            // Act
            var result = await _service.CreateWeeklyMenu(menus);

            // Assert
            Assert.That(result, Is.EqualTo(true));
        }

    }
}
