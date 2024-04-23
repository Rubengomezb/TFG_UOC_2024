using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.API.Controllers;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.CORE.Services.Interfaces;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;

namespace TFG_UOC_2024.TEST.Controller
{
    public class AuthenticationControllerTest
    {
        Mock<IAuthenticationService> mockAuthService;
        AuthenticationController controller;

        [SetUp]
        public void Setup()
        {
            mockAuthService = new Mock<IAuthenticationService>();
            var mockLogger = new Mock<ILogger<AuthenticationController>>();
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

            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            var roleValidators = new List<IRoleValidator<ApplicationRole>>();
            var keyNormalizer = new UpperInvariantLookupNormalizer();
            var errors = new IdentityErrorDescriber();
            var logger = new Mock<ILogger<RoleManager<ApplicationRole>>>();

            var mockRoleManager = new Mock<RoleManager<ApplicationRole>>(
                roleStore.Object,
                roleValidators,
                keyNormalizer,
                errors,
                logger.Object
            );

            var mockDbContext = new Mock<ApplicationContext>();
            var mockUserService = new Mock<IUserService>();
            controller = new AuthenticationController(mockUserManager, mockRoleManager.Object, mockConfig.Object, mockMapper.Object, mockDbContext.Object, mockUserService.Object, mockLogger.Object, mockAuthService.Object);
        }

        [Test]
        public async Task LoginTest()
        {
            var login = new Login();
            var response = new ServiceResponse<UserDTO>();

            mockAuthService.Setup(x => x.Login(login)).ReturnsAsync(response);

            // Act
            var actionResult = await controller.Login(login);

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkObjectResult)));
            var result = ((OkObjectResult)actionResult).Value as UserDTO;
            Assert.That(result, Is.EqualTo(response.Data));
        }

        [Test]
        public async Task ConfirmEmailTest()
        {
            var confirmInput = new ConfirmInput();
            var response = new ServiceResponse<UserDTO>();

            mockAuthService.Setup(x => x.ConfirmEmail(confirmInput)).ReturnsAsync(response);

            // Act
            var actionResult = await controller.ConfirmEmail(confirmInput);

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkObjectResult)));
            var result = ((OkObjectResult)actionResult).Value as UserDTO;
            Assert.That(result, Is.EqualTo(response.Data));
        }

        [Test]
        public async Task ConfirmPasswordTest()
        {
            var confirmInput = new ConfirmInput();
            var response = new ServiceResponse<UserDTO>();

            mockAuthService.Setup(x => x.ConfirmPassword(confirmInput)).ReturnsAsync(response);

            // Act
            var result = await controller.ConfirmPassword(confirmInput);

            // Assert
            Assert.That(result.GetType(), Is.EqualTo(typeof(OkObjectResult)));
            var data = ((OkObjectResult)result).Value as UserDTO;
            Assert.That(data, Is.EqualTo(response.Data));
        }

        // add test for ResetPassword
        [Test]
        public async Task ResetPasswordTest()
        {
            var email = "prueba";
            var response = new GenericResponse();
            mockAuthService.Setup(x => x.ResetPassword(email)).ReturnsAsync(response);

            // Act
            var actionResult = await controller.ResetPassword(email);
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkResult)));
            Assert.That(((OkResult)actionResult).StatusCode, Is.EqualTo(200));
        }

    }
}
