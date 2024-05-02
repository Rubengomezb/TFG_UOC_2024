using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.DTOs;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;
using Microsoft.Maui.Storage;

namespace TFG_UOC_2024.APP.Services
{
    public interface IAuthService
    {
        Task<bool> IsUserAuthenticated();
        Task<UserDTO> LoginAsync(Login dto);
        Task<HttpClient> GetAuthenticatedHttpClientAsync();
        Task<Guid> SignUpAsync(UserInput dto);
        Task<UserSimpleDTO> UpdateUserAsync(string id, UserSimpleDTO dto);
        void Logout();
    }

    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            var serializedData = App.user;
            return serializedData != null;
        }

        public async Task<UserDTO?> LoginAsync(Login dto)
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.PostAsJsonAsync<Login>("api/Authentication/login", dto);

            var content = await response.Content.ReadAsStringAsync();

            var user =
                JsonSerializer.Deserialize<UserDTO>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public async Task<Guid> SignUpAsync(UserInput dto)
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.PostAsJsonAsync<UserInput>("api/User/Post", dto);

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<Guid> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<Guid>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (authResponse.Status == DB.Components.Enums.ServiceStatus.Ok)
            {
                return authResponse.Data;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public void Logout()
        {
            App.user = null;
            if (Preferences.ContainsKey(nameof(App.user)))
            {
                Preferences.Remove(nameof(App.user));
            }
        }

        public async Task<HttpClient> GetAuthenticatedHttpClientAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(AppConstants.HttpClientName);

            var authenticatedUser = App.user;

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authenticatedUser.Token);

            return httpClient;
        }

        public async Task<UserSimpleDTO> UpdateUserAsync(string id, UserSimpleDTO dto)
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.PutAsJsonAsync<UserSimpleDTO>($"api/user/{id}", dto);

            var content = await response.Content.ReadAsStringAsync();
            ServiceResponse<UserSimpleDTO> authResponse =
                JsonSerializer.Deserialize<ServiceResponse<UserSimpleDTO>>(content, new JsonSerializerOptions
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
    }
}
