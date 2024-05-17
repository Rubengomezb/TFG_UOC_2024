using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models.DTOs;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;
using TFG_UOC_2024.CORE.Models;
using System.Text.Json;
using System.Net.Http.Json;

namespace TFG_UOC_2024.INTEGRATION.TEST.Base
{
    public class TestBase
    {
        protected ApiWebApplicationFactory factory;

        protected string AccessToken;

        protected Guid UserId;

        protected HttpClient client;

        private IConfiguration Config;

        public TestBase()
        {
            factory = new ApiWebApplicationFactory();
            this.InitConfig();
            this.client = factory.CreateClient();
            this.Authenticate();
        }

        private void InitConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddXmlFile("appsettings.xml", optional: false, reloadOnChange: true);

            this.Config = builder.Build();
        }

        private void Authenticate()
        {
            var dto = new Login()
            {
                Username = this.Config["User"],
                Password = this.Config["Password"]
            };

            var response = this.client.PostAsJsonAsync("api/Authentication/login", dto).Result;

            var content = response.Content.ReadAsStringAsync().Result;
            var data = JsonSerializer.Deserialize<UserDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            this.AccessToken = data.Token;
            this.UserId = data.Id;
        }
    }
}
