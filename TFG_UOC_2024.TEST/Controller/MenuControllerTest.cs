using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Moq;
using TFG_UOC_2024.API.Controllers;
using TFG_UOC_2024.CORE.Managers.Interfaces;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.ApiModels;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models;
using TFG_UOC_2024.DB.Models.Identity;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.TEST.Controller
{
    public class MenuControllerTest
    {
        [Test]
        public async Task GetWeeklyMenu_ReturnsExpectedMenu1()
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
            var mockMenuManager = new Mock<IMenuManager>();
            var mockLogger = new Mock<ILogger>();
            var mockConfiguration = new Mock<IConfiguration>();

            var startDate = new DateTime(2024, 4, 1);
            var endDate = new DateTime(2024, 4, 7);
            var expectedMenu = new List<MenuDTO>().AsEnumerable();
            var serviceResponse = new ServiceResponse<IEnumerable<MenuDTO>>()
            {
                Data = expectedMenu,
            };

             // Asume que Menu es la clase que retorna GetMenu
            mockMenuManager.Setup(m => m.GetMenu(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(serviceResponse);

            var controller = new MenuController(mockUserManager, mockDbContext.Object, mockMapper.Object, mockMenuManager.Object, mockLogger.Object, mockConfiguration.Object);

            // Act
            ActionResult? actionResult = await controller.GetWeeklyMenu(startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));

            // Assert
            var result = ((OkObjectResult)actionResult).Value as IEnumerable<MenuDTO>; ;
            Assert.IsNotNull(actionResult);
            Assert.That(result, Is.EqualTo(expectedMenu));
        }

        [Test]
        public async Task CreateMenu_ReturnsGenericResponse()
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
            var mockMenuManager = new Mock<IMenuManager>();
            var mockLogger = new Mock<ILogger>();
            var mockConfiguration = new Mock<IConfiguration>();

            var startDate = new DateTime(2024, 4, 1);
            var endDate = new DateTime(2024, 4, 7);
            var genericResp = new ServiceResponse<bool>()
            {
                Status = ServiceStatus.Ok
            };

            // Asume que Menu es la clase que retorna GetMenu
            mockMenuManager.Setup(m => m.CreateMenu(startDate, endDate)).ReturnsAsync(genericResp);

            var controller = new MenuController(mockUserManager, mockDbContext.Object, mockMapper.Object, mockMenuManager.Object, mockLogger.Object, mockConfiguration.Object);

            var request = new CreateMenuRequest()
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var actionResult = await controller.CreateMenu(request);

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkResult)));
            Assert.That(((OkResult)actionResult).StatusCode, Is.EqualTo(200));
        }
    }
}
