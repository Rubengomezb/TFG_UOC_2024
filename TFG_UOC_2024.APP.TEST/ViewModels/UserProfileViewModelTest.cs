using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;
using NUnit.Framework;

namespace TFG_UOC_2024.APP.TEST.ViewModels
{
    public class UserProfileViewModelTest
    {
        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IAuthService>();
            var viewModel = new UserProfileViewModel(service.Object);
            Assert.IsNotNull(viewModel);
        }

        [Test]
        public void UpdateUserProfile()
        {
            App.user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "prueba",
                FirstName = "prueba",
                LastName = "prueba",
                PhoneNumber = "prueba",
                Email = "prueba",
                ContactId = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                FoodType = 1,
            };

            var service = new Mock<IAuthService>();
            var viewModel = new UserProfileViewModel(service.Object);
            var user = new UserSimpleDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "pruebaModificada",
                FirstName = "pruebaModificada",
                LastName = "pruebaModificada",
                PhoneNumber = "pruebaModificada",
            };

            service.Setup(x => x.UpdateUserAsync(It.IsAny<string>(), It.IsAny<UserSimpleDTO>())).ReturnsAsync(user);
            viewModel.LoadUser();
            viewModel.UpdateUser(new UserDTO());
            Assert.IsTrue(viewModel.FirstName == "pruebaModificada");
            Assert.IsTrue(viewModel.LastName == "pruebaModificada");
            Assert.IsTrue(viewModel.PhoneNumber == "pruebaModificada");
        }
    }
}
