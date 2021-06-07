using ADWISER_service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocumentsControllerTests
{
    public class VersionsControllerTests : IClassFixture<ApiWebApplicationFactory>
    {
        public HttpClient Client { get; }

        public VersionsControllerTests(ApiWebApplicationFactory fixture)
        {
            Client = fixture.CreateClient();
        }

        [Fact]
        public async Task Get_TextFile_WhenEditVersion()
        {
            // Arrange.
            var corpusId = "test";
            var textId = "test10";
            var name = "test name";
            var annotation = "some new annotation";
            var source = "some new source";
            var model = new InputTextFileModel
            {
                Name = name,
                Annotation = annotation,
                Source = source,
            };

            var content = JsonConvert.SerializeObject(model);

            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var expectedStatusCode = HttpStatusCode.OK;
            var expectedText = new InputTextFileModel
            {
                Annotation = annotation,
                Name = name,
                Source = source,
            };

            // Act.
            var response = await Client.PatchAsync($"/ADWISER/Documents/{corpusId}/Versions/{textId}", byteContent);
            var result = JsonConvert.DeserializeObject<TextFileModel>(await response.Content.ReadAsStringAsync());

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedText.Name, result.Name);
            Assert.Equal(expectedText.Source, result.Source);
            Assert.Equal(expectedText.Annotation, result.Annotation);
        }

        [Fact]
        public async Task Get_TextFile_WhenAddVersion()
        {
            // Arrange.
            var corpusId = "test";
            var textId = "new id";
            var name = "test name";
            var annotation = "some annotation";
            var source = "some source";
            var model = new InputTextFileModel
            {
                Name = name,
                Annotation = annotation,
                Source = source,
            };

            var content = JsonConvert.SerializeObject(model);

            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var expectedStatusCode = HttpStatusCode.OK;

            // Act.
            var response = await Client.PostAsync($"/ADWISER/Documents/{corpusId}/Versions/", byteContent);

            // Arrange.
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task Get_IntResult_WhenRemoveVersion()
        {
            // Arrange.
            var corpusId = "test";
            var textId = "test10";

            var expectedStatusCode = HttpStatusCode.OK;

            // Act.
            var response = await Client.DeleteAsync($"/ADWISER/Documents/{corpusId}/Versions/{textId}");

            // Arrange.
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task Get_IntResult_WhenMarkText()
        {
            // Arrange.
            var corpusId = "test";
            var textId = "test10";
            var mark = 5;

            var expectedStatusCode = HttpStatusCode.OK;

            var content = JsonConvert.SerializeObject(mark);

            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act.
            var response = await Client.PatchAsync($"/ADWISER/Documents/{corpusId}/Versions/{textId}/Mark", byteContent);

            var result = JsonConvert.DeserializeObject<TextFileModel>(await response.Content.ReadAsStringAsync());

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(mark, result.Mark);
        }

        [Fact]
        public async Task Get_TextFileModelResult_WhenGetLastVersion()
        {
            // Arrange.
            var corpusId = "test";

            var expectedText = new TextFileModel
            {
                Id = "test11",
                Name = "test name 1",
                Source = "aaa",
                Annotation = "bbb",
            };

            var expectedStatusCode = HttpStatusCode.OK;

            // Act.
            var response = await Client.GetAsync($"/ADWISER/Documents/{corpusId}/Versions/last");

            var result = JsonConvert.DeserializeObject<TextFileModel>(await response.Content.ReadAsStringAsync());

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedText.Name, result.Name);
            Assert.Equal(expectedText.Source, result.Source);
            Assert.Equal(expectedText.Annotation, result.Annotation);
        }

        [Fact]
        public async Task Get_TextFileModel_WhenAddCommentary()
        {
            // Arrange.
            var corpusId = "test";
            var textId = "test10";
            var commentary = "some commentary";

            var expectedStatusCode = HttpStatusCode.OK;

            var content = JsonConvert.SerializeObject(commentary);

            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act.
            var response = await Client.PatchAsync($"/ADWISER/Documents/{corpusId}/Versions/{textId}/Commentary", byteContent);

            var result = JsonConvert.DeserializeObject<TextFileModel>(await response.Content.ReadAsStringAsync());

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(commentary, result.Commentary);
        }

        [Fact]
        public async Task Get_TextFileModel_WhenChangeAnnotation()
        {
            // Arrange.
            var corpusId = "test";
            var textId = "test10";
            var annotation = "some annotation";

            var expectedStatusCode = HttpStatusCode.OK;

            var content = JsonConvert.SerializeObject(annotation);

            var buffer = Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Act.
            var response = await Client.PatchAsync($"/ADWISER/Documents/{corpusId}/Versions/{textId}/Annotation", byteContent);

            var result = JsonConvert.DeserializeObject<TextFileModel>(await response.Content.ReadAsStringAsync());

            // Assert.
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(annotation, result.Annotation);
        }
    }
}
