using ADWISER_service.Models;
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
        private string URI { get => "https://localhost:44301/Storage/"; }

        private string CollectionName { get => "test"; }

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
            HttpClient client = new HttpClient();

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

            HttpResponseMessage response = await client.PostAsync(uri, byteContent);

            string contentString = await response.Content.ReadAsStringAsync();

            var corpus = BsonSerializer.Deserialize<MongoDBCorpus>(contentString);
            return new CorpusModel
            {
                Id = corpus.Id,
                Author = corpus.Author,
                Date = corpus.Date,
                Documents = new List<TextFileModel>(),
                Name = corpus.Name,
            };
        }

        public async Task<TextFileModel> AddTextToCorpus(string id, string set, InputTextFileModel textFile/*string name, string source, string annotation*/)
        {
            HttpClient client = new HttpClient();

            string uri = URI + $"collections/{CollectionName}/documents/{id}?set={set}";

            var content = JsonConvert.SerializeObject(textFile);

            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PostAsync(uri, byteContent);

            string contentString = await response.Content.ReadAsStringAsync();

            //var corpus = BsonSerializer.Deserialize<MongoDBCorpus>(contentString);
            var corpus = await GetCorpusById(id);
            var text = corpus.Last();
            //var text = corpus.Documents.First();
            //var text = BsonSerializer.Deserialize<MongoDBTextFile>(contentString);
            return new TextFileModel
            {
                Id = text.Id,
                Annotation = text.Annotation,
                Date = text.Date,
                Name = text.Name,
                Source = text.Source,
            };
        }

        public async Task<List<CorpusModel>> GetCorporaFromStorage()
        {
            HttpClient client = new HttpClient();

            string uri = URI + $"collections/{CollectionName}";

            HttpResponseMessage response = await client.GetAsync(uri);

            string contentString = await response.Content.ReadAsStringAsync();

            var corpora = BsonSerializer.Deserialize<List<MongoDBCorpus>>(contentString);
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

        public async Task<CorpusModel> RemoveTextFromCorpus(string set, string corpusId, string textId)
        {
            HttpClient client = new HttpClient();

            string uri = URI + $"collections/{CollectionName}/documents/{corpusId}/{textId}?set={set}";

            HttpResponseMessage response = await client.DeleteAsync(uri);

            string contentString = await response.Content.ReadAsStringAsync();

            var corpus = BsonSerializer.Deserialize<MongoDBCorpus>(contentString);
            return new CorpusModel
            {
                Author = corpus.Author,
                Date = corpus.Date,
                Documents = corpus.Documents.Select(x => new TextFileModel
                {
                    Annotation = x.Annotation,
                    Date = x.Date,
                    Id = x.Id,
                    Name = x.Name,
                    Source = x.Source,
                }),
                Id = corpus.Id,
                Name = corpus.Name,
            };
        }

        public async Task<TextFileModel> EditTextCorpus(string corpusId, string textId, string set, string field, string text)
        {
            HttpClient client = new HttpClient();

            string uri = URI + $"collections/{CollectionName}/documents/{corpusId}/{textId}?set={set}&field={field}";

            var content = JsonConvert.SerializeObject(text);

            var buffer = System.Text.Encoding.UTF8.GetBytes(content);
            var byteContent = new ByteArrayContent(buffer);

            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await client.PatchAsync(uri, byteContent);

            string contentString = await response.Content.ReadAsStringAsync();

            return new TextFileModel();
            /*var corpus = BsonSerializer.Deserialize<>(contentString);
            return new CorpusModel
            {
                Author = corpus.Author,
                Date = corpus.Date,
                Documents = corpus.Documents.Select(x => new TextFileModel
                {
                    Annotation = x.Annotation,
                    Date = x.Date,
                    Id = x.Id,
                    Name = x.Name,
                    Source = x.Source,
                }),
                Id = corpus.Id,
                Name = corpus.Name,
            };*/
        }
    }
}
