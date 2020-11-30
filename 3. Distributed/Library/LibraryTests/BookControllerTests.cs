using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;

namespace LibraryTests 
{
    public class BookControllerTests : IClassFixture<WebApplicationFactory<LibraryServer.Startup>>
    {
        private readonly WebApplicationFactory<LibraryServer.Startup> factory;
        public BookControllerTests(WebApplicationFactory<LibraryServer.Startup> factory) 
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetAllBookTest()
        {
            var client = factory.CreateClient();

            var s = await client.GetStringAsync("api/books");
            var books = JsonConvert.DeserializeObject<LibraryContracts.Book[]>(s);
            Assert.Equal(1, books.Length);
        }
    }    
}