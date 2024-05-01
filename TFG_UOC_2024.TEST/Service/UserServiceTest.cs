using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.WSTrust;
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
using TFG_UOC_2024.CORE.Services.User;
using TFG_UOC_2024.DB.Context;
using TFG_UOC_2024.DB.Models.Identity;
using TFG_UOC_2024.DB.Repository.Interfaces;
using TFG_UOC_2024.TEST.Setting;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;
using static TFG_UOC_2024.DB.Components.Enums;

namespace TFG_UOC_2024.TEST.Service
{
    public class UserServiceTest
    {
        public IUserService userService;


        [SetUp]
        public void SetUp()
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

            var mockUserRepository = new Mock<IUserRepository>();
            var mockUserRoleRepository = new Mock<IUserRoleRepository>();
            var mockContactRepository = new Mock<IContactRepository>();

            var mockDbContext = new Mock<ApplicationContext>();
            var mockUserService = new Mock<IUserService>();
            userService = new UserService(
                mockConfig.Object,
                mockDbContext.Object,
                mockUserManager,
                mockRoleManager.Object,
                mockUserRepository.Object,
                mockUserRoleRepository.Object,
                mockContactRepository.Object, AutomapperSingleton.Mapper, mockLogger.Object, null);
        }

        // mock for 
        [Test]
        public void SearchUsersTest()
        {
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockConfig = new Mock<IConfiguration>();
            var mockUserSearchInput = new Mock<UserSearchInput>();
            var mockUser = new Mock<IEnumerable<ApplicationUser>>();
            var mockUserQueryable = new Mock<IQueryable<ApplicationUser>>();
            var mockUserSearchDto = new UserSearchDTO();

            Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();

            mockUserRepository.Setup(x => x.GetUsers()).Returns(mockUser.Object);
            mockUserRepository.Setup(x => x.GetAll()).Returns(mockUserQueryable.Object);

            // Act
            var result = userService.SearchUsers(mockUserSearchInput.Object);

            // Assert
            Assert.That(result.Data.GetType(), Is.EqualTo(mockUserSearchDto.GetType()));
        }

        //add test for Task<ServiceResponse<Guid>> AddUser(UserInput dto) of UserService
        [Test]
        public async Task AddUserTest()
        {
            var id = Guid.NewGuid();
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockUserInput = new UserInput()
            {
                Id = id,
                Password = "password",
                UserName = "username",
            };
            var mockUser = new ApplicationUser();
            var mockUserSearchDto = new UserSearchDTO();
            var mockServiceResponse = new ServiceResponse<UserSearchDTO>();
            var mockusermanager = new Mock<UserManager<ApplicationUser>>();
            var identity = new Mock<IdentityResult>();

            var userStoreMock = new Mock<IUserPasswordStore<ApplicationUser>>();
            var user = new ApplicationUser();
            var password = "TuContraseña";

            userStoreMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            mockUserManager.Setup(x => x.CreateAsync(mockUser, password)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await userService.AddUser(mockUserInput);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Data, Is.EqualTo((Guid)default));
        }

        //add test for UpdateUser(string id, UserSimpleDTO dto)
        [Test]
        public async Task UpdateUserTest()
        {
            var id = Guid.NewGuid();
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockUserSimpleDto = new UserSimpleDTO()
            {
                Id = id,
                UserName = "username",
            };
            var mockUser = new ApplicationUser();
            var mockUserSearchDto = new UserSearchDTO();
            var mockServiceResponse = new ServiceResponse<UserSearchDTO>();
            var mockusermanager = new Mock<UserManager<ApplicationUser>>();
            var identity = new Mock<IdentityResult>();

            var userStoreMock = new Mock<IUserPasswordStore<ApplicationUser>>();
            var user = new ApplicationUser();
            var password = "TuContraseña";

            userStoreMock.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                               userStoreMock.Object, null, null, null, null, null, null, null, null);

            mockusermanager.Setup(x => x.CreateAsync(mockUser, password)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await userService.UpdateUser(id.ToString(), mockUserSimpleDto);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.BadRequest));
        }

        //add test for DeleteUser(string id)
        [Test]
        public async Task DeleteUserTest()
        {
            var id = Guid.NewGuid();
            // Arrange
            var mockUserRepository = new Mock<IUserRepository>();
            var mockUserSimpleDto = new UserSimpleDTO()
            {
                Id = id,
                UserName = "username",
            };
            var mockUser = new ApplicationUser();
            var mockUserSearchDto = new UserSearchDTO();
            var mockServiceResponse = new ServiceResponse<UserSearchDTO>();
            var mockusermanager = new Mock<UserManager<ApplicationUser>>();
            var identity = new Mock<IdentityResult>();

            var userStoreMock = new Mock<IUserPasswordStore<ApplicationUser>>();
            var user = new ApplicationUser();
            var password = "TuContraseña";

            userStoreMock.Setup(x => x.FindByIdAsync(id.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var mockUserManager = new Mock<UserManager<ApplicationUser>>(
                                              userStoreMock.Object, null, null, null, null, null, null, null, null);

            mockUserManager.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync(user);

            // Act
            var result = await userService.DeleteUser(id.ToString());

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Status, Is.EqualTo(ServiceStatus.NotFound));
        }
    }
}
