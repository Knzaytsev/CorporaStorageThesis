using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorporaStorage.Entities
{
    public class MongoDBContext : IDBContext
    {
        private string ConnectionString { get => "mongodb+srv://adugeen:adugeen@cluster0.xbrqy.mongodb.net/test?retryWrites=true&w=majority"; }

        private string DatabaseName { get => "test"; }

        private MongoClient Client { get; set; }

        private IMongoDatabase Database { get; }

        public MongoDBContext()
        {
            var credential = MongoCredential.CreateCredential("admin", "adugeen", "adugeen");
            Client = new MongoClient(ConnectionString);
            Database = Client.GetDatabase(DatabaseName);
        }

        public async Task<object> AddCorpusToCollection(string collectionName, object corpus)
        {
            var collection = Database.GetCollection<BsonDocument>(collectionName);
            var document = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(corpus.ToString());
            await collection.InsertOneAsync(document);
            return document;
        }

        public async Task<object> AddTextToCorpus(string collectionName, string set, string id, object text)
        {
            var collection = Database.GetCollection<BsonDocument>(collectionName);
            var mongoText = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(text.ToString());

            mongoText.Add(new BsonElement("_id", ObjectId.GenerateNewId()));

            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var update = Builders<BsonDocument>.Update.AddToSet(set, mongoText);

            var result = await collection.FindOneAndUpdateAsync(filter, update);
            //return GetCorpusById(collectionName, id);
            return result;
        }

        public async Task<object> GetCorpusFromCollection(string collectionName)
        {
            var collection = Database.GetCollection<BsonDocument>(collectionName);
            return (await collection.FindAsync(new BsonDocument())).ToList();
        }

        public async Task<object> RemoveCorpusFromCollection(string collectionName, string attribute, string criteria)
        {
            var collection = Database.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq(attribute, ObjectId.Parse(criteria));
            await collection.DeleteOneAsync(filter);
            return collection.Find(new BsonDocument()).ToList();
        }

        public async Task<object> UpdateCorpusInCollection(string collectionName, object corpus, string id, string attribute)
        {
            var collection = Database.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var document = corpus.ToString();//MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(corpus.ToString());
            var update = Builders<BsonDocument>.Update.Set(attribute, document);
            await collection.UpdateOneAsync(filter, update);
            return (await collection.FindAsync(filter)).ToJson();
        }

        public async Task<object> GetCorpusById(string collectionName, string id)
        {
            var collection = Database.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            var document = await collection.FindAsync(filter);
            return await document.ToListAsync();
        }

        public async Task<object> RemoveTextFromCorpus(string collectionName, string set, string corpusId, string textId)
        {
            var collection = Database.GetCollection<BsonDocument>(collectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(corpusId));
            var update = Builders<BsonDocument>.Update.PullFilter(set, Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(textId)));
            await collection.UpdateOneAsync(filter, update);
            return await GetCorpusById(collectionName, corpusId);
        }

        public async Task<object> UpdateTextInCorpus(string collectionName, string corpusId, string textId, string set, string field, string text)
        {
            var collection = Database.GetCollection<BsonDocument>(collectionName);
            //var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(corpusId));
            //var update = Builders<BsonDocument>.Update.Set(set, Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(textId)));

            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(corpusId))
                & Builders<BsonDocument>.Filter.Eq($"{set}._id", ObjectId.Parse(textId));/*, Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(textId)));*/

            var update = Builders<BsonDocument>.Update.Set($"{set}.$.{field}", text);

            var result = await collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount;
        }
    }
}
