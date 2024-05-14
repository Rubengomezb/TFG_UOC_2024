using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;

namespace TFG_UOC_2024.APP.TEST.ViewModels
{
    public class SignUpViewModelTest
    {
        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IAuthService>();
            var viewModel = new SignUpViewModel(service.Object);
            Assert.IsNotNull(viewModel);
        }

        [Test]
        public async Task SignUpTest()
        {
            var service = new Mock<IAuthService>();


            var viewModel = new SignUpViewModel(service.Object);
            viewModel.forTest = true;
            viewModel.Username = "prueba";
            viewModel.Password = "prueba";
            viewModel.Email = "prueba";
            viewModel.FirstName = "prueba";
            viewModel.LastName = "prueba";
            viewModel.PhoneNumber = "prueba";

            service.Setup(x => x.SignUpAsync(It.IsAny<UserInput>())).ReturnsAsync(Guid.NewGuid);
            await viewModel.SignUp();
            Assert.IsTrue(viewModel.Username == "prueba");
            Assert.IsTrue(viewModel.Password == "prueba");
            Assert.IsTrue(viewModel.Email == "prueba");
            Assert.IsTrue(viewModel.FirstName == "prueba");
            Assert.IsTrue(viewModel.LastName == "prueba");
            Assert.IsTrue(viewModel.PhoneNumber == "prueba");
        }
    }
}
