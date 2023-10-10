using Microsoft.AspNetCore.Mvc.Testing;
using Chirp.Repository;

namespace Chirp.Razor.Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _fixture;
        private readonly HttpClient _client;

        public IntegrationTests(WebApplicationFactory<Program> fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
        }

        [Fact]
        public async void CanSeePublicTimeline()
        {
            HttpResponseMessage response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Chirp!", content);
            Assert.Contains("Public Timeline", content);
        }

        [Theory]
        [InlineData("Helge")]
        [InlineData("Rasmus")]
        public async void CanSeePrivateTimeline(string author)
        {
            HttpResponseMessage response = await _client.GetAsync($"/{author}");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Chirp!", content);
            Assert.Contains($"{author}'s Timeline", content);
        }

        [Fact]
        public void Test()
        {
            CheepRepository cr = new CheepRepository();
            cr.GetCheeps();

            Assert.True(true);
        }
    }
}