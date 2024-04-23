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
using static TFG_UOC_2024.CORE.Models.DTOs.ContactPropertyDTO;

namespace TFG_UOC_2024.INTEGRATION.TEST.Controllers
{
    public class UserControllerTest : TestBase
    {
        [Test]
        public async Task GetUsers()
        {
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await client.GetAsync("api/user");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<UserSearchDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(users.Users.Count > 0);
        }

        [Test]
        public async Task GetUser()
        {
            var id = Guid.Parse("08dc3b98-e0a2-40ff-8fdc-ebde9f148a15");
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var endpoint = $"api/user/{id}";
            var response = await client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(user != null);
        }

        [Test]
        public async Task GetUserRoles()
        {
            var id = Guid.Parse("08dc62ea-5b91-4385-8f7b-b2bcf19f79a1");
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var endpoint = $"api/user/{id}/roles";
            var response = await client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var roles = JsonSerializer.Deserialize<IEnumerable<UserRoleDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.IsTrue(roles.ToList().Count() > 0);
        }

        [Test]
        public async Task AddUser()
        {
            var id = Guid.Parse("08dc3b98-e0a2-40ff-8fdc-ebde9f148a15");
            var user = new UserInput
            {
                Id = id,
                UserName = "testuser",
                Email = "test",
                FirstName = "test",
                LastName = "test",
                PhoneNumber = "666666666",
            };
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await client.PostAsJsonAsync("api/user", user);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var newUser = JsonSerializer.Deserialize<Guid>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(newUser);
        }

        [Test]
        public async Task UpdateUser()
        {
            var id = Guid.Parse("08dc3b98-e0a2-40ff-8fdc-ebde9f148a15");
            var user = new UserSimpleDTO
            {
                Id = id,
                UserName = "testuser",
                Email = "test2",
                FirstName = "test2",
                LastName = "test",
                PhoneNumber = "666666666",
            };
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await client.PutAsJsonAsync($"api/user/{id}", user);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var updatedUser = JsonSerializer.Deserialize<UserSimpleDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            Assert.IsNotNull(updatedUser);
            Assert.That(updatedUser.Email, Is.EqualTo("test2"));
            Assert.That(updatedUser.FirstName, Is.EqualTo("test2"));
        }

        [Test]
        public async Task DeleteUser()
        {
            var id = Guid.Parse("08dc3b98-e0a2-40ff-8fdc-ebde9f148a15");
            this.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            var response = await client.DeleteAsync($"api/user/{id}");
            response.EnsureSuccessStatusCode();
            Assert.That(HttpStatusCode.OK, Is.EqualTo(response.StatusCode));
        }
    }
}
