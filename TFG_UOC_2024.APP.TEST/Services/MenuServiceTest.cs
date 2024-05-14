using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.APP.ViewModels;
using TFG_UOC_2024.CORE.Models.DTOs;

namespace TFG_UOC_2024.APP.TEST.Services
{
    public class MenuServiceTest
    {
        [Test]
        public void ConstructorTest()
        {
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new MenuService(service.Object);
            Assert.IsNotNull(viewModel);
        }

        [Test]
        public void CreateWeeklyMenuAsyncTest()
        {
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new MenuService(service.Object);
            var result = viewModel.CreateWeeklyMenuAsync(DateTime.Now, DateTime.Now);
            Assert.IsNotNull(result);
        }

        [Test]
        public void GetWeeklyMenuAsyncTest()
        {
            var service = new Mock<IHttpClientFactory>();
            var viewModel = new MenuService(service.Object);
            var result = viewModel.GetWeeklyMenuAsync(DateTime.Now, DateTime.Now);
            Assert.IsNotNull(result);
        }
    }
}
