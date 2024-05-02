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
                EndDate = sunday
            };

            var httpClient = await GetAuthenticatedHttpClientAsync();

            var response = await httpClient.PostAsJsonAsync("api/Menu/menu", requestDates);

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<bool> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<bool>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return authResponse.Data;
        }

        public async Task<List<MenuDTO>> GetWeeklyMenuAsync(DateTime monday, DateTime sunday)
        {
            var httpClient = await GetAuthenticatedHttpClientAsync();

            var response = await httpClient.GetAsync($"api/Menu/menu");

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<List<MenuDTO>> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<List<MenuDTO>>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (authResponse.Status == DB.Components.Enums.ServiceStatus.Ok)
            {
                return authResponse.Data;
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
