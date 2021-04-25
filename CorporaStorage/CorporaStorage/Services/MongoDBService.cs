using CorporaStorage.Entities;
using CorporaStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace CorporaStorage.Services
{
    public class MongoDBService : IStorageService
    {
        private IDBContext Context;

        public MongoDBService(IDBContext context)
        {
            Context = context;
        }

        public async Task<string> AddCorpusToCorporaStorage(CollectionModel model)
        {
            var result = await Context.AddCorpusToCollection(model.CollectionName, model.DataModel);
            return result.ToString();
        }

        public async Task<string> GetCorporaFromCollection(string collection)
        {
            var result = await Context.GetCorpusFromCollection(collection) as List<BsonDocument>;
            return result.ToJson();
        }

        public async Task<string> RemoveCorpusFromCollection(CriteriaModel model)
        {
            var result = await Context.RemoveCorpusFromCollection(model.Collection, model.Attribute, model.Criteria);
            return result.ToJson();
        }

        public async Task<string> UpdateCorpusInCollection(CriteriaModel criteria, CollectionModel collection)
        {
            var result = await Context.UpdateCorpusInCollection(collection.CollectionName, collection.DataModel, criteria.Id, criteria.Attribute);
            return result.ToJson();
        }

        public async Task<string> GetCorpusById(string collectionName, string id)
        {
            var result = await Context.GetCorpusById(collectionName, id) as List<BsonDocument>;
            return result.First().ToJson();
        }

        public async Task<string> AddTextToCorpus(string collectionName, string id, string set, object text)
        {
            var result = await Context.AddTextToCorpus(collectionName, set, id, text);
            return result.ToString();
        }

        public async Task<string> RemoveText(string collectionName, string corpusId, string textId, string set)
        {
            var result = await Context.RemoveTextFromCorpus(collectionName, set, corpusId, textId) as List<BsonDocument>;
            return result.First().ToJson();
        }

        public async Task<string> UpdateTextInCorpus(string collectionName, string corpusId, string textId, string set, string field, string text)
        {
            var result = await Context.UpdateTextInCorpus(collectionName, corpusId, textId, set, field, text);
            return result.ToJson();
        }
    }
}
