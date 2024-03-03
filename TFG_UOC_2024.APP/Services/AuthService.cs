using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TFG_UOC_2024.CORE.Models;
using TFG_UOC_2024.CORE.Models.DTOs;
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;

namespace TFG_UOC_2024.APP.Services
{
    public interface IAuthService
    {
        Task<bool> IsUserAuthenticated();
        Task<string?> LoginAsync(Login dto);
        Task<UserDTO> GetAuthenticatedUserAsync();
        Task<HttpClient> GetAuthenticatedHttpClientAsync();
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
            var serializedData = await SecureStorage.Default.GetAsync(AppConstants.AuthStorageKeyName);
            return !string.IsNullOrWhiteSpace(serializedData);
        }

        public async Task<UserDTO> GetAuthenticatedUserAsync()
        {
            var serializedData = await SecureStorage.Default.GetAsync(AppConstants.AuthStorageKeyName);
            if (!string.IsNullOrWhiteSpace(serializedData))
            {
                return JsonSerializer.Deserialize<UserDTO>(serializedData);
            }
            return null;
        }

        public async Task<string?> LoginAsync(Login dto)
        {
            var httpClient = _httpClientFactory.CreateClient("Client");

            var response = await httpClient.PostAsJsonAsync<Login>("api/auth/login", dto);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ServiceResponse<UserDTO> authResponse =
                    JsonSerializer.Deserialize<ServiceResponse<UserDTO>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (authResponse.Status == DB.Components.Enums.ServiceStatus.Ok)
                {
                    var serializedData = JsonSerializer.Serialize(authResponse.Data);
                    await SecureStorage.SetAsync(AppConstants.AuthStorageKeyName, serializedData);
                }
                else
                {
                    return authResponse.Status.ToString();
                }
            }
            else
            {
                return "Error in logging in";
            }
            return null;
        }

        public void Logout() => SecureStorage.Default.Remove(AppConstants.AuthStorageKeyName);

        public async Task<HttpClient> GetAuthenticatedHttpClientAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(AppConstants.HttpClientName);

            var authenticatedUser = await GetAuthenticatedUserAsync();

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", authenticatedUser.Token);

            return httpClient;
        }
    }
}
