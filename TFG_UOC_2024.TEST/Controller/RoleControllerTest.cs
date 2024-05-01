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
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.TEST.Controller
{
    public class RoleControllerTest
    {
        // add setup for controller and mock
        public RoleController controller;
        public Mock<IRoleService> roleServiceMock;

        [SetUp]
        public void SetUp()
        {
            roleServiceMock = new Mock<IRoleService>();
            var mockLogger = new Mock<ILogger<RoleController>>();
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
            controller = new RoleController(mockUserManager, mockRoleManager.Object, mockConfig.Object, mockMapper.Object, mockDbContext.Object, roleServiceMock.Object, mockLogger.Object);
        }

        [Test]
        public async Task GetRole_ReturnsRole()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedRole = new List<RoleDTO>();
            var serviceResponse = new ServiceResponse<List<RoleDTO>>()
            {
                Data = expectedRole,
            };

            // Asume that Role is the class that returns GetRole
            roleServiceMock.Setup(m => m.GetAll()).Returns(serviceResponse);

            // Act
            var actionResult = controller.Get();

            // Assert
            var result = ((OkObjectResult)actionResult).Value as List<RoleDTO>;
            Assert.That(result, Is.EqualTo(expectedRole));
        }

        // add test for Get(id)
        [Test]
        public async Task GetRoleById_ReturnsRole()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedRole = new RoleDTO();
            var serviceResponse = new ServiceResponse<RoleDTO>()
            {
                Data = expectedRole,
            };

            // Asume that Role is the class that returns GetRole
            roleServiceMock.Setup(m => m.Get(id)).ReturnsAsync(serviceResponse);

            // Act
            var actionResult = await controller.Get(id);

            // Assert
            var result = ((OkObjectResult)actionResult).Value as RoleDTO;
            Assert.That(result, Is.EqualTo(expectedRole));
        }

        // add test for Post(role)
        [Test]
        public async Task AddRole_ReturnsRole()
        {
            // Arrange
            var role = new RoleDTO();
            var expectedRole = new Guid();
            var serviceResponse = new ServiceResponse<Guid>()
            {
                Data = expectedRole,
            };

            // Asume that Role is the class that returns GetRole
            roleServiceMock.Setup(m => m.Add(role)).ReturnsAsync(serviceResponse);

            // Act
            var actionResult = await controller.Post(role);

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkObjectResult)));
            Assert.That(((OkObjectResult)actionResult).StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task UpdateRole_ReturnsRole()
        {
            // Arrange
            var id = Guid.NewGuid();
            var role = new RoleDTO();
            var serviceResponse = new GenericResponse()
            {
                Status = ServiceStatus.Ok,
            };

            // Asume that Role is the class that returns GetRole
            roleServiceMock.Setup(m => m.Update(id, role)).ReturnsAsync(serviceResponse);

            // Act
            var actionResult = await controller.Put(id, role);

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkResult)));
            Assert.That(((OkResult)actionResult).StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task DeleteRole_ReturnsRole()
        {
            // Arrange
            var roleid = Guid.NewGuid();
            var serviceResponse = new GenericResponse()
            {
                Status = ServiceStatus.Ok,
            };

            // Asume that Role is the class that returns GetRole
            roleServiceMock.Setup(m => m.DeleteRole(roleid)).ReturnsAsync(serviceResponse);

            // Act
            var actionResult = await controller.DeleteRole(roleid);

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkResult)));
            Assert.That(((OkResult)actionResult).StatusCode, Is.EqualTo(200));
        }

        // add test for GetClaims
        [Test]
        public async Task GetClaims_ReturnsClaims()
        {
            // Arrange
            var expectedClaims = new List<ClaimDTO>();
            var serviceResponse = new ServiceResponse<List<ClaimDTO>>()
            {
                Data = expectedClaims,
            };

            // Asume that Role is the class that returns GetRole
            roleServiceMock.Setup(m => m.GetClaims()).Returns(serviceResponse);

            // Act
            var actionResult = controller.GetClaims();

            // Assert
            var result = ((OkObjectResult)actionResult).Value as List<ClaimDTO>;
            Assert.That(result, Is.EqualTo(expectedClaims));
        }

        // add test for UpdateClaims
        [Test]
        public async Task UpdateClaims_ReturnsClaims()
        {
            // Arrange
            var roleid = Guid.NewGuid();
            var claimValues = new List<string>();
            var serviceResponse = new GenericResponse()
            {
                Status = ServiceStatus.Ok,
            };

            // Asume that Role is the class that returns GetRole
            roleServiceMock.Setup(m => m.UpdateClaims(roleid, claimValues)).ReturnsAsync(serviceResponse);

            // Act
            var actionResult = await controller.UpdateClaims(roleid, claimValues);

            // Assert
            Assert.That(actionResult.GetType(), Is.EqualTo(typeof(OkResult)));
            Assert.That(((OkResult)actionResult).StatusCode, Is.EqualTo(200));
        }
    }
}
