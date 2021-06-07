using ADWISER_service.Models;
using ADWISER_service.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentsControllerTests
{
    public class ApiWebApplicationFactory : WebApplicationFactory<ADWISER_service.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // will be called after the `ConfigureServices` from the Startup
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IStorageService, MongoDBServiceStub>();
            });
        }
    }

    public class MongoDBServiceStub : IStorageService
    {
        private List<CorpusModel> Models = new List<CorpusModel>();

        public MongoDBServiceStub()
        {
            Models = GetDocuments();
        }

        public Task<CorpusModel> AddCorpusToCollection(string name, string author)
        {
            var list = GetDocuments();
            return Task.Run(() => list.First());
        }

        public Task<int> AddTextToCorpus(string id, string set, InputTextFileModel textModel)
        {
            var result = 1;
            try
            {
                var versions = Models.SingleOrDefault(x => x.Id == id).Documents.ToList();
                versions.Add(new TextFileModel
                {
                    Id = "new id",
                    Name = textModel.Name,
                    Annotation = textModel.Annotation,
                    Source = textModel.Source,
                    Date = textModel.Date,
                });
            }
            catch
            {
                result = 0;
            }
            
            return Task.Run(() => result);
        }

        public Task<int> EditTextCorpus(string corpusId, string textId, string set, string field, string text)
        {
            var version = Models.SingleOrDefault(x => x.Id == corpusId).Documents.SingleOrDefault(x => x.Id == textId);
            var result = 1;
            switch (field)
            {
                case "Source":
                    version.Source = text;
                    break;
                case "Annotation":
                    version.Annotation = text;
                    break;
                case "Mark":
                    version.Mark = int.Parse(text);
                    break;
                case "Commentary":
                    version.Commentary = text;
                    break;
                default:
                    result = 0;
                    break;
            }
            return Task.Run(() => result);
        }

        public Task<List<TextFileModel>> GetCorpusById(string id)
        {
            var documents = Models;
            return Task.Run(() => documents.SingleOrDefault(x => x.Id == id).Documents.ToList());
        }

        public Task<int> RemoveCorpusFromCollection(string corpusId)
        {
            var documents = GetDocuments();

            var document = documents.SingleOrDefault(x => x.Id == corpusId);
            return Task.Run(() => documents.Remove(document) ? 1 : 0);
        }

        public Task<int> RemoveTextFromCorpus(string set, string corpusId, string textId)
        {
            var result = 1;
            try
            {
                var document = Models.SingleOrDefault(x => x.Id == corpusId);
                var version = document.Documents.SingleOrDefault(x => x.Id == textId);
                document.Documents.ToList().Remove(version);
            }
            catch
            {
                result = 0;
            }
            return Task.Run(() => result);
        }


        Task<List<CorpusModel>> IStorageService.GetCorporaFromStorage()
        {
            return Task.Run(() => GetDocuments());
        }

        private List<CorpusModel> GetDocuments()
        {
            return new List<CorpusModel>
            {
                new CorpusModel
                {
                    Id = "test",
                    Author = "test author",
                    Name = "test name",
                    Documents = new List<TextFileModel>
                    {
                        new TextFileModel
                        {
                            Id = "test10",
                            Name = "test name",
                            Source = "abcde",
                            Annotation = "edcba",
                        },
                        new TextFileModel
                        {
                            Id = "test11",
                            Name = "test name 1",
                            Source = "aaa",
                            Annotation = "bbb",
                        }
                    },
                },
                new CorpusModel
                {
                    Id = "another test",
                    Author = "another author",
                    Name = "another name",
                    Documents = new List<TextFileModel>(),
                },
            };
        }
    }
}
