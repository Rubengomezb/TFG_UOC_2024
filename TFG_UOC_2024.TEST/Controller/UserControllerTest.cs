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
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.TEST.Controller
{
    public class UserControllerTest
    {
        public UserController controller;
        public Mock<IUserService> userServiceMock;

        [SetUp]
        public void SetUp()
        {
            userServiceMock = new Mock<IUserService>();
            var mockLogger = new Mock<ILogger<UserController>>();
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
            controller = new UserController(mockUserManager, mockLogger.Object, mockConfig.Object, mockMapper.Object, mockDbContext.Object, userServiceMock.Object);
        }

        [Test]
        public async Task Get_ReturnsUsers()
        {
            var expectedUsers = new UserSearchDTO();
            var serviceResponse = new ServiceResponse<UserSearchDTO>()
            {
                Data = expectedUsers,
            };

            userServiceMock.Setup(m => m.SearchUsers(It.IsAny<UserSearchInput>())).Returns(serviceResponse);

            var result = controller.Get(new UserSearchInput());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        //add test Get(Guid id)
        [Test]
        public async Task Get_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var expectedUser = new UserSimpleDTO();
            var serviceResponse = new ServiceResponse<UserSimpleDTO>()
            {
                Data = expectedUser,
            };

            userServiceMock.Setup(m => m.GetUser(id)).ReturnsAsync(serviceResponse);

            var result = await controller.Get(id);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Post_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var serviceResponse = new ServiceResponse<Guid>()
            {
                Data = id,
            };

            userServiceMock.Setup(m => m.AddUser(It.IsAny<UserInput>())).ReturnsAsync(serviceResponse);

            var result = await controller.Post(new UserInput());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Put_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var expectedUser = new UserSimpleDTO();
            var serviceResponse = new ServiceResponse<UserSimpleDTO>()
            {
                Data = expectedUser,
            };

            userServiceMock.Setup(m => m.UpdateUser(id.ToString(), It.IsAny<UserSimpleDTO>())).ReturnsAsync(serviceResponse);

            var result = await controller.Put(id.ToString(), new UserSimpleDTO());

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task DeleteUser_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var genericResp = new GenericResponse()
            {
                Status = ServiceStatus.Ok
            };

            userServiceMock.Setup(m => m.DeleteUser(id.ToString())).ReturnsAsync(genericResp);

            var result = await controller.DeleteUser(id.ToString());

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task UnDeleteUser_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var genericResp = new GenericResponse()
            {
                Status = ServiceStatus.Ok
            };

            userServiceMock.Setup(m => m.UnDeleteUser(id.ToString())).ReturnsAsync(genericResp);

            var result = await controller.UnDeleteUser(id.ToString());

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task UpdatePassword_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var expectedUser = new UserSimpleDTO();
            var serviceResponse = new ServiceResponse<UserSimpleDTO>()
            {
                Data = expectedUser,
            };

            userServiceMock.Setup(m => m.UpdatePassword(id.ToString(), It.IsAny<PasswordChangeInput>())).ReturnsAsync(serviceResponse);

            var result = await controller.UpdatePassword(id.ToString(), new PasswordChangeInput());

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void GetUserRoles_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var expectedUser = new List<UserRoleDTO>();
            var serviceResponse = new ServiceResponse<IEnumerable<UserRoleDTO>>()
            {
                Data = expectedUser,
            };

            userServiceMock.Setup(m => m.GetUserRoles(id)).Returns(serviceResponse);

            var result = controller.GetUserRoles(id);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task AddRoleToUser_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var expectedUser = new UserRoleDTO();
            var serviceResponse = new ServiceResponse<UserRoleDTO>()
            {
                Data = expectedUser,
            };

            userServiceMock.Setup(m => m.AddRoleToUser(id, It.IsAny<UserRoleInput>())).ReturnsAsync(serviceResponse);

            var result = await controller.AddRoleToUser(id, new UserRoleInput());

            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task DeleteRoleFromUser_ReturnsUser()
        {
            var id = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var genericResp = new GenericResponse()
            {
                Status = ServiceStatus.Ok
            };

            userServiceMock.Setup(m => m.DeleteRoleFromUser(id, roleId)).ReturnsAsync(genericResp);

            var result = await controller.DeleteRoleFromUser(id, roleId);

            Assert.IsInstanceOf<OkResult>(result);
        }
    }
}
