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

        /// <summary>
        /// Возвращает список документов с текстами пользователей.
        /// </summary>
        /// <param name="collectionName">Наименование коллекции, в которой содержатся документы.</param>
        /// <param name="corpus">Документ, полученный от клиента.</param>
        /// <returns>Только что добавленный документ.</returns>
        /// <response code="200">Сервис успешно добавил документ в коллекцию.</response>
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

        /// <summary>
        /// Получает список документов в конкретной коллекции.
        /// </summary>
        /// <param name="collectionName">Наименование коллекции, в которой содержатся документы.</param>
        /// <returns>Строка, содержащая список документов коллекции.</returns>
        /// <response code="200">Сервис успешно получил документы.</response>
        [HttpGet]
        [Route("collections/{collectionName}")]
        public async Task<string> GetCollection([FromRoute] string collectionName)
        {
            return await Service.GetCorporaFromCollection(collectionName);
        }

        /// <summary>
        /// Удаляет документ с текстами из коллекции.
        /// </summary>
        /// <param name="collectionName">Наименование коллекции, в которой содержатся документы с текстами.</param>
        /// <param name="attribute">Наименование атрибута, по которому происходит фильтрация.</param>
        /// <param name="criteria">Значение атрибута.</param>
        /// <returns>Строка, содержащая список документа без удалённого документа.</returns>
        /// <response code="200">Сервис успешно удалил документ из коллекции.</response>
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

        /// <summary>
        /// Обновляет документ в коллекции по заданному атрибуту.
        /// </summary>
        /// <param name="collectionName">Наименование коллекции, содержащей документы с текстами.</param>
        /// <param name="id">Идентификатор документа коллекции.</param>
        /// <param name="attribute">Атрибут, по которому осуществляется обновление поля документа.</param>
        /// <param name="corpus">Обновляемое содержание поля.</param>
        /// <returns>Строка, содержащая обновлённый документ.</returns>
        /// <response code="200">Сервис успешно обновил документ.</response>
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

        /// <summary>
        /// Получает документ по его идентификатору.
        /// </summary>
        /// <param name="collectionName">Наименование коллекции, содержащей список документов с текстами.</param>
        /// <param name="id">Идентификатор документа.</param>
        /// <returns>Строка, содержащая искомый документ.</returns>
        /// <response code="200">Сервис успешно получил документ из БД.</response>
        [HttpGet]
        [Route("collections/{collectionName}/{id}")]
        public async Task<object> GetCorpusById([FromRoute] string collectionName, [FromRoute] string id)
        {
            return await Service.GetCorpusById(collectionName, id);
        }

        /// <summary>
        /// Добавляет текст в конкретный документ.
        /// </summary>
        /// <param name="collectionName">Наименование коллекции, содержащей список документов с текстами.</param>
        /// <param name="id">Идентификатор документа.</param>
        /// <param name="text">Содержание добавляемого текста.</param>
        /// <param name="set">Коллекция документа, в которую добавляется текст.</param>
        /// <returns>Добавляемый текст.</returns>
        /// <response code="200">Сервис успешно добавил текст в коллекцию документа.</response>
        [HttpPost]
        [Route("collections/{collectionName}/documents/{id}")]
        public async Task<object> AddText([FromRoute] string collectionName, [FromRoute] string id, [FromBody] object text, [FromQuery] string set)
        {
            return await Service.AddTextToCorpus(collectionName, id, set, text);
        }

        /// <summary>
        /// Удаляет текст из коллекции текстов документа.
        /// </summary>
        /// <param name="collectionName">Наименование коллекции, содержащей список документов с текстами.</param>
        /// <param name="corpusId">Идентификатор документа.</param>
        /// <param name="textId">Идентификатор текста.</param>
        /// <param name="set">Наименование коллекции документа, по которой удаляется искомый документ.</param>
        /// <returns>Количество затронутых текстов.</returns>
        /// <response code="200">Сервис успешно удалил текст из документа.</response>
        [HttpDelete]
        [Route("collections/{collectionName}/documents/{corpusId}/{textId}")]
        public async Task<object> RemoveText([FromRoute] string collectionName, [FromRoute] string corpusId, [FromRoute] string textId, [FromQuery] string set)
        {
            return await Service.RemoveText(collectionName, corpusId, textId, set);
        }

        /// <summary>
        /// Обновляет текст в коллекции документа.
        /// </summary>
        /// <param name="collectionName">Наименование коллекции, содержащей список документов с текстами.</param>
        /// <param name="corpusId">Идентификатор документа.</param>
        /// <param name="textId">Идентификатор текста.</param>
        /// <param name="set">Наименование коллекции документа, по которой удаляется искомый документ.</param>
        /// <param name="field">Обновляемое поле текста.</param>
        /// <param name="text">Содержание поля.</param>
        /// <returns>Количество затронутых полей.</returns>
        /// <response code="200">Сервис успешно обновил поле текста.</response>
        [HttpPatch]
        [Route("collections/{collectionName}/documents/{corpusId}/{textId}")]
        public async Task<object> UpdateText([FromRoute] string collectionName, [FromRoute] string corpusId, [FromRoute] string textId, [FromQuery] string set, [FromQuery] string field, [FromBody] string text)
        {
            return await Service.UpdateTextInCorpus(collectionName, corpusId, textId, set, field, text);
        }
    }
}
