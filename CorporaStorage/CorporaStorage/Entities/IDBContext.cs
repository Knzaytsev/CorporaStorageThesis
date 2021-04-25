using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorporaStorage.Entities
{
    public interface IDBContext
    {
        public Task<object> AddCorpusToCollection(string collectionName, object corpus);

        public Task<object> GetCorpusFromCollection(string collectionName);

        public Task<object> RemoveCorpusFromCollection(string collectionName, string attribute, string criteria);

        public Task<object> UpdateCorpusInCollection(string collectionName, object corpus, string id, string attribute);

        public Task<object> GetCorpusById(string collectionName, string id);

        public Task<object> AddTextToCorpus(string collectionName, string set, string id, object text);

        public Task<object> RemoveTextFromCorpus(string collectionName, string set, string corpusId, string textId);

        public Task<object> UpdateTextInCorpus(string collectionName, string corpusId, string textId, string set, string field, string text);
    }
}
