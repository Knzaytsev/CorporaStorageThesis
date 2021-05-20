using ADWISER_service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ADWISER_service.Services
{
    public class MongoDBService : IStorageService
    {
        private string URI { get => Configuration["StorageServiceURI"]; }

        private string CollectionName { get => Configuration["CollectionName"]; }

        private HttpClient Client { get => new HttpClient(); }

        private IConfiguration Configuration { get; }

        public MongoDBService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<List<TextFileModel>> GetCorpusById(string id)
        {
            HttpClient client = new HttpClient();

            string uri = URI + $"collections/{CollectionName}/{id}";

            HttpResponseMessage response = await client.GetAsync(uri);

            string contentString = await response.Content.ReadAsStringAsync();
            var corpus = BsonSerializer.Deserialize<MongoDBCorpus>(contentString);
            return corpus.Documents.Select(x => new TextFileModel
            {
                Id = x.Id,
                Annotation = x.Annotation,
                Date = x.Date,
                Name = x.Name,
                Source = x.Source,
            }).ToList();
        }

        public async Task<CorpusModel> AddCorpusToCollection(string author, string name)
        {
            string uri = URI + $"collections/{CollectionName}/documents";

            var content = JsonConvert.SerializeObject(new InputCorpusModel
            {
                Author = author,
                Name = name,
                Date = DateTime.Now,
                Documents = new List<TextFileModel>(),
            });

            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await Client.PostAsync(uri, byteContent);

            string contentString = await response.Content.ReadAsStringAsync();

            MongoDBCorpus corpus;
            try
            {
                corpus = BsonSerializer.Deserialize<MongoDBCorpus>(contentString);
            }
            catch
            {
                throw new Exception("Unable to deserialize object.");
            }
            
            return new CorpusModel
            {
                Id = corpus.Id,
                Author = corpus.Author,
                Date = corpus.Date,
                Documents = new List<TextFileModel>(),
                Name = corpus.Name,
            };
        }

        public async Task<int> AddTextToCorpus(string id, string set, InputTextFileModel textFile)
        {
            string uri = URI + $"collections/{CollectionName}/documents/{id}?set={set}";

            var content = JsonConvert.SerializeObject(textFile);

            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await Client.PostAsync(uri, byteContent);

            string contentString = await response.Content.ReadAsStringAsync();

            var result = BsonSerializer.Deserialize<int>(contentString);

            return result;
        }

        public async Task<List<CorpusModel>> GetCorporaFromStorage()
        {
            string uri = URI + $"collections/{CollectionName}";

            HttpResponseMessage response = await Client.GetAsync(uri);

            string contentString = await response.Content.ReadAsStringAsync();

            List<MongoDBCorpus> corpora;
            try
            {
                corpora = BsonSerializer.Deserialize<List<MongoDBCorpus>>(contentString);
            }
            catch
            {
                throw new Exception("Unable to deserialize one of the element.");
            }
            return corpora.Select(x => new CorpusModel
            {
                Id = x.Id,
                Author = x.Author,
                Date = x.Date,
                Name = x.Name,
                Documents = x.Documents.Select(y => new TextFileModel
                {
                    Annotation = y.Annotation,
                    Date = y.Date,
                    Id = y.Id,
                    Name = y.Name,
                    Source = y.Source,
                }),
            }).ToList();
        }

        public async Task<int> RemoveTextFromCorpus(string set, string corpusId, string textId)
        {
            string uri = URI + $"collections/{CollectionName}/documents/{corpusId}/{textId}?set={set}";

            HttpResponseMessage response = await Client.DeleteAsync(uri);

            string contentString = await response.Content.ReadAsStringAsync();

            var result = BsonSerializer.Deserialize<int>(contentString);
            return result;
        }

        public async Task<int> EditTextCorpus(string corpusId, string textId, string set, string field, string text)
        {
            string uri = URI + $"collections/{CollectionName}/documents/{corpusId}/{textId}?set={set}&field={field}";

            var content = JsonConvert.SerializeObject(text);

            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await Client.PatchAsync(uri, byteContent);

            string contentString = await response.Content.ReadAsStringAsync();

            var result = BsonSerializer.Deserialize<int>(contentString);

            return result;
        }

        public async Task<int> RemoveCorpusFromCollection(string corpusId)
        {
            string uri = URI + $"collections/{CollectionName}?attribute=_id&criteria={corpusId}";

            HttpResponseMessage response = await Client.DeleteAsync(uri);

            string contentString = await response.Content.ReadAsStringAsync();

            var result = BsonSerializer.Deserialize<int>(contentString);

            return result;
        }

        public async Task<int> MarkText(string corpusId, uint mark)
        {
            var texts = await GetCorpusById(corpusId);
            var text = texts.Last();

            var uri = URI + $"collections/{CollectionName}/documents/{text.Id}?set=Documents&field=Mark";

            var content = JsonConvert.SerializeObject(mark);

            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await Client.PatchAsync(uri, byteContent);

            string contentString = await response.Content.ReadAsStringAsync();

            var result = BsonSerializer.Deserialize<int>(contentString);

            return result;
        }
    }
}
