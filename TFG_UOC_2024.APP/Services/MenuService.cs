using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.ApiModels;

namespace TFG_UOC_2024.APP.Services
{
    public interface IMenuService
    {
        Task<bool> CreateWeeklyMenuAsync(DateTime monday, DateTime sunday);
    }

    public class MenuService : IMenuService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MenuService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateWeeklyMenuAsync(DateTime monday, DateTime sunday)
        {
            var requestDates = new CreateMenuRequest()
            {
                StartDate = monday,
                EndDate = sunday
            };

            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.PostAsJsonAsync("api/menu/menu", requestDates);

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<bool> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<bool>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return authResponse.Data;
        }
    }
}
