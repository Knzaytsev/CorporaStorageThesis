using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorporaStorage.Models;
using CorporaStorage.Services;

namespace CorporaStorage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : Controller
    {
        private IStorageService Service { get; set; }

        public StorageController(IStorageService service)
        {
            Service = service;
        }

        [HttpPost]
        [Route("collections/{collectionName}/documents")]
        public async Task<string> CreateDocument([FromRoute] string collectionName, [FromBody] object corpus)
        {
            var model = new CollectionModel
            {
                CollectionName = collectionName,
                DataModel = corpus,
            };
            return await Service.AddCorpusToCorporaStorage(model);
        }

        [HttpGet]
        [Route("collections/{collectionName}")]
        public async Task<string> GetCollection([FromRoute] string collectionName)
        {
            return await Service.GetCorporaFromCollection(collectionName);
        }

        [HttpDelete]
        [Route("collections/{collectionName}")]
        public async Task<string> DeleteCorpus([FromRoute] string collectionName, [FromQuery] string attribute, [FromQuery] string criteria)
        {
            var model = new CriteriaModel
            {
                Collection = collectionName,
                Attribute = attribute,
                Criteria = criteria,
            };
            return await Service.RemoveCorpusFromCollection(model);
        }

        [HttpPatch]
        [Route("collections/{collectionName}")]
        public async Task<string> UpdateCorpus([FromRoute] string collectionName,[FromQuery] string id, [FromQuery] string attribute, [FromBody] object corpus)
        {
            var criteriaModel = new CriteriaModel
            {
                Collection = collectionName,
                Attribute = attribute,
                Id = id,
            };

            var collectionModel = new CollectionModel
            {
                CollectionName = collectionName,
                DataModel = corpus,
            };

            return await Service.UpdateCorpusInCollection(criteriaModel, collectionModel);
        }

        [HttpGet]
        [Route("collections/{collectionName}/{id}")]
        public async Task<object> GetCorpusById([FromRoute] string collectionName, [FromRoute] string id)
        {
            return await Service.GetCorpusById(collectionName, id);
        }

        [HttpPost]
        [Route("collections/{collectionName}/documents/{id}")]
        public async Task<object> AddText([FromRoute] string collectionName, [FromRoute] string id, [FromBody] object text, [FromQuery] string set)
        {
            return await Service.AddTextToCorpus(collectionName, id, set, text);
        }

        [HttpDelete]
        [Route("collections/{collectionName}/documents/{corpusId}/{textId}")]
        public async Task<object> RemoveText([FromRoute] string collectionName, [FromRoute] string corpusId, [FromRoute] string textId, [FromQuery] string set)
        {
            return await Service.RemoveText(collectionName, corpusId, textId, set);
        }

        [HttpPatch]
        [Route("collections/{collectionName}/documents/{corpusId}/{textId}")]
        public async Task<object> UpdateText([FromRoute] string collectionName, [FromRoute] string corpusId, [FromRoute] string textId, [FromQuery] string set, [FromQuery] string field, [FromBody] string text)
        {
            return await Service.UpdateTextInCorpus(collectionName, corpusId, textId, set, field, text);
        }
    }
}
