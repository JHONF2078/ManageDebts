using Xunit;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FluentAssertions;

namespace ManageDebts.Presentation.IntegrationTests
{
    public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AccountControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenValidData()
        {
            // Arrange: email único por ejecución para evitar colisiones
            var email = $"integration_{Guid.NewGuid():N}@test.com";
            var payload = new
            {
                email,
                fullName = "Integration User",
                password = "Password.123" // cumple longitud mínima = 8
            };
            var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/account/register", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK, responseBody);

            var json = JObject.Parse(responseBody);
            json["token"]!.Value<string>().Should().NotBeNullOrWhiteSpace();
            json["refreshToken"]!.Value<string>().Should().NotBeNullOrWhiteSpace();
            json["email"]!.Value<string>().Should().Be(email);
        }
    }
}
