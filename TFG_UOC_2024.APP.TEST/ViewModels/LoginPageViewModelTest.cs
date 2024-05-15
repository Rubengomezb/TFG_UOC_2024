using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;
using NUnit.Framework;

namespace TFG_UOC_2024.APP.TEST.ViewModels
{
    public class LoginPageViewModelTest
    {
        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IAuthService>();
            var viewModel = new LoginPageViewModel(service.Object);
            Assert.IsNotNull(viewModel);
        }

        [Test]
        public async Task LoginTest()
        {
            var service = new Mock<IAuthService>();

            var viewModel = new LoginPageViewModel(service.Object);

            var user = new UserDTO()
            {
                Id = Guid.NewGuid(),
                UserName = "prueba",
            };

            var login = new Login()
            {
                Username = "prueba",
                Password = "prueba"
            };

            service.Setup(x => x.LoginAsync(It.IsAny<Login>())).ReturnsAsync(user);
            viewModel.UserName = "prueba";
            viewModel.Password = "prueba";
            viewModel.forTest = true;
            await viewModel.Login();

            Assert.IsNotNull(App.user);
        }
    }
}
