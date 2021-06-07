using ADWISER_service.Models;
using ADWISER_service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Controllers
{
    [ApiController]
    [Route("ADWISER/{controller}")]
    public class DocumentsController : Controller
    {
        private IStorageService Service { get; set; }

        public DocumentsController(IStorageService service)
        {
            Service = service;
        }

        /// <summary>
        /// Создаёт документ с версиями текстов.
        /// </summary>
        /// <param name="Name">Название документа.</param>
        /// <param name="Author">Логин создателя документа.</param>
        /// <returns>Возвращает только что созданный документ.</returns>
        /// <response code="200">Документ успешно создан.</response>
        /// <response code="400">В процессе создания возникли проблемы.</response>
        [HttpPost]
        public async Task<ActionResult<CorpusModel>> CreateDocument([FromQuery] string Name, [FromQuery] string Author)
        {
            try
            {
                return await Service.AddCorpusToCollection(Author, Name);
            }
            catch
            {
                return StatusCode(400);
            }
            
        }

        /// <summary>
        /// Получает версии документа.
        /// </summary>
        /// <param name="id">Идентификатор документа.</param>
        /// <returns>Возвращает список версий.</returns>
        /// <response code="200">Версии были получены успешно.</response>
        /// <response code="400">В процессе получения возникли проблемы.</response>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<List<TextFileModel>>> GetDocumentVersions([FromRoute] string id)
        {
            try
            {
                return await Service.GetCorpusById(id);
            }
            catch
            {
                return StatusCode(200);
            }
        }

        /// <summary>
        /// Получает все документы в системе.
        /// </summary>
        /// <returns>Возвращает список документов.</returns>
        /// <response code="200">Список был получен успешно.</response>
        /// <response code="400">В процессе получения возникли проблемы.</response>
        [HttpGet]
        public async Task<ActionResult<List<CorpusModel>>> GetDocuments()
        {
            try
            {
                return await Service.GetCorporaFromStorage();
            }
            catch
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Получает документы конкретного пользователя.
        /// </summary>
        /// <param name="User">Логин пользователя.</param>
        /// <returns>Возвращает список документов пользователя.</returns>
        /// <response code="200">Список был получен успешно.</response>
        /// <response code="400">В процессе получения возникли проблемы.</response>
        [HttpGet]
        [Route("user/{User}")]
        public async Task<ActionResult<List<CorpusModel>>> GetDocumentsByUser([FromRoute] string User)
        {
            try
            {
                var corpora = await Service.GetCorporaFromStorage();
                return corpora.Where(x => x.Author == User).ToList();
            }
            catch
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Удаляет документ.
        /// </summary>
        /// <param name="id">Идентификатор документа.</param>
        /// <returns>Возвращает список документов без удалённого документа.</returns>
        /// <response code="200">Документ был удалён успешно.</response>
        /// <response code="400">В процессе удаления возникли проблемы.</response>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<CorpusModel>>> DeleteDocument(string id)
        {
            try
            {
                var result = await Service.RemoveCorpusFromCollection(id);
                if (result == 0)
                {
                    return StatusCode(400);
                }
                return await Service.GetCorporaFromStorage();
            }
            catch
            {
                return StatusCode(400);
            }
        }
    }
}
