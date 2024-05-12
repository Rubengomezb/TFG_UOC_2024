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
using TFG_UOC_2024.CORE.Models.DTOs;
using System.Net.Http.Headers;

namespace TFG_UOC_2024.APP.Services
{
    public interface IMenuService
    {
        Task<bool> CreateWeeklyMenuAsync(DateTime monday, DateTime sunday);
        Task<List<MenuDTO>> GetWeeklyMenuAsync(DateTime monday, DateTime sunday);
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
                EndDate = sunday,
                FoodType = App.user.FoodType,
            };

            var httpClient = await GetAuthenticatedHttpClientAsync();

            var response = await httpClient.PostAsJsonAsync("api/Menu/menu", requestDates);

            var content = await response.Content.ReadAsStringAsync();

            bool authResponse =
                JsonSerializer.Deserialize<bool>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return authResponse;
        }

        public async Task<List<MenuDTO>> GetWeeklyMenuAsync(DateTime monday, DateTime sunday)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync();
            var response = await httpClient.GetAsync($"api/Menu/menu?startDate={monday.ToString("yyy-MM-dd")}&endDate={sunday.ToString("yyy-MM-dd")}", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false); ;

            var content = await response.Content.ReadAsStringAsync();
            List<MenuDTO> authResponse =
                JsonSerializer.Deserialize<List<MenuDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return authResponse;
            }
            else
            {
                return null;
            }
        }

        public async Task<HttpClient> GetAuthenticatedHttpClientAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var authenticatedUser = App.user;

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authenticatedUser.Token);

            return httpClient;
        }
    }
}
