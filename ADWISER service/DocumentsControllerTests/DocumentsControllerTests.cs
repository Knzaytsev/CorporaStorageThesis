using ADWISER_service.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace DocumentsControllerTests
{
    public class DocumentsControllerTests : IClassFixture<ApiWebApplicationFactory>
    {
        public HttpClient Client { get; }

        public DocumentsControllerTests(ApiWebApplicationFactory fixture)
        {
            Client = fixture.CreateClient();
        }

        [Fact]
        public async Task Get_ListWithCorpusModels_WhenGetDocuments()
        {
            // Arrange.
            var expectedStatusCode = HttpStatusCode.OK;
            var expectedModels = new List<CorpusModel>
            {
                new CorpusModel(),
                new CorpusModel(),
            };

            // Act.
            var response = await Client.GetAsync("/ADWISER/Documents");
            var result = JsonConvert.DeserializeObject<List<CorpusModel>>(await response.Content.ReadAsStringAsync());

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedModels.Count, result.Count);
        }

        [Fact]
        public async Task Get_CorpusModel_WhenAddDocument()
        {
            // Arrange.
            var author = "test author";
            var name = "test name";
            var expectedStatusCode = HttpStatusCode.OK;
            var expectedCount = 2;
            var expectedModel = new CorpusModel
            {
                Id = "test",
                Author = author,
                Name = name,
                Documents = new List<TextFileModel>(),
            };

            // Act.
            var response = await Client.PostAsync($"/ADWISER/Documents?Name={author}&Author={name}", null);
            var result = JsonConvert.DeserializeObject<CorpusModel>(await response.Content.ReadAsStringAsync());

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedModel.Id, result.Id);
            Assert.Equal(expectedModel.Name, result.Name);
            Assert.Equal(expectedModel.Author, result.Author);
            Assert.Equal(expectedCount, result.Documents.Count());
        }

        [Fact]
        public async Task Get_ListWithTextFiles_WhenGetCorpusById()
        {
            // Arrange.
            var id = "test";
            var expectedStatusCode = HttpStatusCode.OK;
            var expectedCount = 2;

            // Act.
            var response = await Client.GetAsync($"/ADWISER/Documents/{id}");
            var result = JsonConvert.DeserializeObject<List<TextFileModel>>(await response.Content.ReadAsStringAsync());

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public async Task Get_StatusCode_WhenRemoveDocument()
        {
            // Arrange.
            var id = "test";
            var expectedStatusCode = HttpStatusCode.OK;

            // Act.
            var response = await Client.DeleteAsync($"/ADWISER/Documents/{id}");

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task Get_ListWithCorpusModels_WhenGetDocumentByUser()
        {
            // Arrange.
            var id = "test author";
            var expectedStatusCode = HttpStatusCode.OK;
            var expectedModels = new List<CorpusModel>
            {
                new CorpusModel
                {
                    Author = "test author",
                },
            };

            // Act.
            var response = await Client.GetAsync($"/ADWISER/Documents/user/{id}");
            var result = JsonConvert.DeserializeObject<List<CorpusModel>>(await response.Content.ReadAsStringAsync());

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedModels.Count, result.Count);
            Assert.Equal(expectedModels.First().Author, result.First().Author);
        }
    }
}
