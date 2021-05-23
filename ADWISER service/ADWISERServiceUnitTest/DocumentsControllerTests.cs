using ADWISER_service.Controllers;
using ADWISER_service.Services;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Xunit;

namespace ADWISERServiceUnitTest
{
    //[TestClass]
    public class DocumentsControllerTests : IClassFixture<WebApplicationFactory<ADWISER_service.Startup>>
    {
        public HttpClient Client { get; }

        public DocumentsControllerTests() { }

        public DocumentsControllerTests(WebApplicationFactory<ADWISER_service.Startup> fixture)
        {
            Client = fixture.CreateClient();
        }

        [Fact]
        //[TestMethod]
        public async Task TestMethod()
        {
            var response = await Client.GetAsync("/ADWISER/Documents");
            response.EnsureSuccessStatusCode();

            //Assert.AreEqual(0, 0);
        }
    }
}
