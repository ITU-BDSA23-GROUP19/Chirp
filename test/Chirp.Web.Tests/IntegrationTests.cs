namespace Chirp.Web.Tests
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public IntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = true, HandleCookies = true });
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
        [InlineData("Jacqualine Gilcoine")]
        public async void CanSeePrivateTimeline(string author)
        {
            HttpResponseMessage response = await _client.GetAsync($"/{author}");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Chirp!", content);
            Assert.Contains($"{author}'s Timeline", content);
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/?page=-1")]
        [InlineData("/?page=0")]
        [InlineData("/?page=1")]
        [InlineData("/?page=hello")]
        public async void CanSeeFirstPageMatchPublicTimeline(string page)
        {
            HttpResponseMessage response = await _client.GetAsync(page);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Chirp!", content);
            Assert.Contains("Public Timeline", content);

            string messageAuthor1 = "Quintin Sitts";
            string messageText1 = "On reaching the end of either, there came a sound so deep an influence over her?";
            string messageTimestamp1 = "01-08-2023 13:17:14";

            Assert.Contains(messageAuthor1, content);
            Assert.Contains(messageText1, content);
            Assert.Contains(messageTimestamp1, content);

            string messageAuthor2 = "Roger Histand";
            string messageText2 = "I have the truth out of all other explanations are more busy than yourself.";
            string messageTimestamp2 = "01-08-2023 13:17:08";

            Assert.Contains(messageAuthor2, content);
            Assert.Contains(messageText2, content);
            Assert.Contains(messageTimestamp2, content);

            string messageAuthor3 = "Octavio Wagganer";
            string messageText3 = "It was a sawed-off shotgun; so he fell back dead.";
            string messageTimestamp3 = "01-08-2023 13:17:01";

            Assert.Contains(messageAuthor3, content);
            Assert.Contains(messageText3, content);
            Assert.Contains(messageTimestamp3, content);
        }

        [Theory]
        [InlineData("/Jacqualine%20Gilcoine")]
        [InlineData("/Jacqualine%20Gilcoine/?page=-1")]
        [InlineData("/Jacqualine%20Gilcoine/?page=0")]
        [InlineData("/Jacqualine%20Gilcoine/?page=1")]
        [InlineData("/Jacqualine%20Gilcoine/?page=hello")]
        public async void CanSeeFirstPageMatchPrivateTimeline(string page)
        {
            HttpResponseMessage response = await _client.GetAsync(page);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Chirp!", content);
            Assert.Contains("Jacqualine Gilcoine's Timeline", content);

            string messageAuthor1 = "Jacqualine Gilcoine";
            string messageText1 = "With back to my friend, patience!";
            string messageTimestamp1 = "01-08-2023 13:16:58";

            Assert.Contains(messageAuthor1, content);
            Assert.Contains(messageText1, content);
            Assert.Contains(messageTimestamp1, content);

            string messageAuthor2 = "Jacqualine Gilcoine";
            string messageText2 = "I sat down at the moor-gate where he was.";
            string messageTimestamp2 = "01-08-2023 13:16:41";

            Assert.Contains(messageAuthor2, content);
            Assert.Contains(messageText2, content);
            Assert.Contains(messageTimestamp2, content);

            string messageAuthor3 = "Jacqualine Gilcoine";
            string messageText3 = "Now, amid the cloud-scud.";
            string messageTimestamp3 = "01-08-2023 13:16:30";

            Assert.Contains(messageAuthor3, content);
            Assert.Contains(messageText3, content);
            Assert.Contains(messageTimestamp3, content);
        }

        [Fact]
        public async void CanSeeRightAmountOfCheepsPerPagePublicTimeline()
        {
            HttpResponseMessage response = await _client.GetAsync("/");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            int occurences = content.Split("<li>").Length - 1;

            Assert.Contains("Chirp!", content);
            Assert.Contains("Public Timeline", content);

            Assert.Equal(32, occurences);
        }

        [Fact]
        public async void CanSeeRightAmountOfCheepsPerPagePrivateTimeline()
        {
            HttpResponseMessage response = await _client.GetAsync("/Jacqualine%20Gilcoine");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            int occurences = content.Split("<li>").Length - 1;

            Assert.Contains("Chirp!", content);
            Assert.Contains("Jacqualine Gilcoine's Timeline", content);

            Assert.Equal(32, occurences);
        }
    }
}