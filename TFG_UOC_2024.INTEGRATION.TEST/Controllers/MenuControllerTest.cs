using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models.DTOs;
using TFG_UOC_2024.INTEGRATION.TEST.Base;

namespace TFG_UOC_2024.INTEGRATION.TEST.Controllers
{
    public class MenuControllerTest : TestBase
    {
        //add integration tests here
        public MenuControllerTest() { }

        [Test]
        public async Task GetWeeklyMenu()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/menu/menu?startDate={DateTime.Now.ToString("yyyy-MM-dd")}&endDate={DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")}";
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var menu = JsonSerializer.Deserialize<IEnumerable<MenuDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(menu != null);
        }

        [Test]
        public async Task CreateMenu()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var request = $"api/menu/menu";
            var response = await client.PostAsJsonAsync(request, new { startDate = DateTime.Now, endDate = DateTime.Now.AddDays(7) });
            response.EnsureSuccessStatusCode();
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
