using CorporaStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorporaStorage.Services
{
    public interface IStorageService
    {
        public Task<string> AddCorpusToCorporaStorage(CollectionModel model);

        public Task<string> GetCorporaFromCollection(string collection);

        public Task<string> RemoveCorpusFromCollection(CriteriaModel model);

        public Task<string> UpdateCorpusInCollection(CriteriaModel criteria, CollectionModel collection);

        public Task<string> GetCorpusById(string collectionName, string id);

        public Task<string> AddTextToCorpus(string collectionName, string id, string set, object text);

        public Task<string> RemoveText(string collectionName, string corpusId, string textId, string set);

        public Task<string> UpdateTextInCorpus(string collectionName, string corpusId, string textId, string set, string field, string text);
    }
}
