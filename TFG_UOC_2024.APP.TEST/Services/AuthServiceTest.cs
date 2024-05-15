using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using TFG_UOC_2024.APP.Services;
using TFG_UOC_2024.CORE.Models.DTOs;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;

namespace TFG_UOC_2024.APP.TEST.Services
{
    public class AuthServiceTest
    {
        [Test]
        public void ConstructorTest()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var authService = new AuthService(httpClientFactory.Object);
            Assert.IsNotNull(authService);
        }

        [Test]
        public void IsUserAuthenticatedTest()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var authService = new AuthService(httpClientFactory.Object);
            var result = authService.IsUserAuthenticated();
            Assert.IsNotNull(result);
        }

        [Test]
        public void LoginAsyncTest()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var authService = new AuthService(httpClientFactory.Object);
            var result = authService.LoginAsync(new Login());
            Assert.IsNotNull(result);
        }
        [Test]
        public void GetAuthenticatedHttpClientAsyncTest()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var authService = new AuthService(httpClientFactory.Object);
            var result = authService.GetAuthenticatedHttpClientAsync();
            Assert.IsNotNull(result);
        }
        [Test]
        public void SignUpAsyncTest()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var authService = new AuthService(httpClientFactory.Object);
            var result = authService.SignUpAsync(new UserInput());
            Assert.IsNotNull(result);
        }

        [Test]
        public void UpdateUserAsyncTest()
        {
            var httpClientFactory = new Mock<IHttpClientFactory>();
            var authService = new AuthService(httpClientFactory.Object);
            var result = authService.UpdateUserAsync("id", new UserSimpleDTO());
            Assert.IsNotNull(result);
        }
    }
}
