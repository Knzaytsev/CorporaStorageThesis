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
    [Route("ADWISER/Documents/{CorpusId}/{controller}")]
    public class VersionsController : Controller
    {
        private IStorageService Service { get; set; }

        public VersionsController(IStorageService service)
        {
            Service = service;
        }

        /// <summary>
        /// Редактирует версию документа.
        /// </summary>
        /// <param name="corpusId">Идентификатор документа.</param>
        /// <param name="textId">Идентификатор версии.</param>
        /// <param name="model">Модель входных данных версии.</param>
        /// <returns>Возвращает отредактированную версию.</returns>
        /// <response code="200">Версия была отредактирована успешно.</response>
        /// <response code="400">В процессе редактирования возникли проблемы.</response>
        [HttpPatch]
        [Route("{TextId}")]
        public async Task<ActionResult<TextFileModel>> EditVersion([FromRoute] string corpusId, [FromRoute] string textId, [FromBody] InputTextFileModel model)
        {
            var result = await Service.EditTextCorpus(corpusId, textId, "Documents", "Source", model.Source);
            if (result == 0)
            {
                return StatusCode(400);
            }
            result = await Service.EditTextCorpus(corpusId, textId, "Documents", "Annotation", model.Annotation);
            if (result == 0)
            {
                return StatusCode(400);
            }
            try
            {
                var corpus = await Service.GetCorpusById(corpusId);
                return corpus.SingleOrDefault(x => x.Id == textId);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Добавляет версию документа.
        /// </summary>
        /// <param name="CorpusId">Идентификатор документа.</param>
        /// <param name="text">Модель входных данных версии.</param>
        /// <returns>Возвращает добавленную модель данных.</returns>
        /// <response code="200">Версия была добавлена успешна.</response>
        /// <response code="400">В процессе добавления возникли проблемы.</response>
        [HttpPost]
        public async Task<ActionResult<TextFileModel>> AddVersion([FromRoute] string CorpusId, [FromBody] InputTextFileModel text)
        {
            var result = await Service.AddTextToCorpus(CorpusId, "Documents", text);
            if (result == 0)
            {
                return StatusCode(400);
            }
            try
            {
                return (await Service.GetCorpusById(CorpusId)).Last();
            }
            catch
            {
                return StatusCode(400);
            }
            
        }

        /// <summary>
        /// Удаляет версию документа.
        /// </summary>
        /// <param name="corpusId">Идентификатор документа.</param>
        /// <param name="textId">Идентификатор версии.</param>
        /// <returns>Возрващает список версий без учёта удалённой.</returns>
        /// <response code="200">Версия была удалена успешно.</response>
        /// <response code="400">В процессе удаления возникли проблемы.</response>
        [HttpDelete]
        [Route("{textId}")]
        public async Task<ActionResult<List<TextFileModel>>> RemoveVersion([FromRoute] string corpusId, [FromRoute] string textId)
        {
            var result = await Service.RemoveTextFromCorpus("Documents", corpusId, textId);
            if (result == 0)
            {
                return StatusCode(400);
            }
            try
            {
                return (await Service.GetCorpusById(corpusId));
            }
            catch
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Ставит оценку заданию.
        /// </summary>
        /// <param name="CorpusId">Идентификатор документа.</param>
        /// <param name="textId">Идентификатор текста.</param>
        /// <param name="mark">Оценка тексту.</param>
        /// <returns>Возвращает версию с поставленной оценкой.</returns>
        /// <response code="200">Оценка была добавлена успешно.</response>
        /// <response code="400">В процессе добавления возникли проблемы.</response>
        [HttpPatch]
        [Route("{textId}/Mark")]
        public async Task<ActionResult<TextFileModel>> MarkTask([FromRoute] string CorpusId, [FromRoute] string textId, [FromBody] int mark)
        {
            if (mark < 0 || mark > 10)
            {
                return StatusCode(400);
            }
            var result = await Service.EditTextCorpus(CorpusId, textId, "Documents", "Mark", mark.ToString());
            if (result == 0)
            {
                return StatusCode(400);
            }
            try
            {
                return (await Service.GetCorpusById(CorpusId)).SingleOrDefault(x => x.Id == textId);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Получает последнюю версию документа.
        /// </summary>
        /// <param name="CorpusId">Идентификатор документа.</param>
        /// <returns>Возвращает последнюю версию документа.</returns>
        /// <response code="200">Версия была получена успешно.</response>
        /// <response code="400">В процессе получения возникли проблемы.</response>
        [HttpGet]
        [Route("last")]
        public async Task<ActionResult<TextFileModel>> GetLastVersion([FromRoute] string CorpusId)
        {
            try
            {
                var version = await Service.GetCorpusById(CorpusId);
                return version.Last();
            }
            catch
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Добавляет комментарий к версии.
        /// </summary>
        /// <param name="CorpusId">Идентификатор документа.</param>
        /// <param name="textId">Идентификатор версии.</param>
        /// <param name="commentary">Добавляемый комментарий.</param>
        /// <returns>Возвращает версию с добавленным комментарием.</returns>
        /// <response code="200">Комментарий был добавлен успешно.</response>
        /// <response code="400">В процессе добавления возникли проблемы.</response>
        [HttpPatch]
        [Route("{textId}/Commentary")]
        public async Task<ActionResult<TextFileModel>> AddCommentary([FromRoute] string CorpusId, [FromRoute] string textId, [FromBody] string commentary)
        {
            var result = await Service.EditTextCorpus(CorpusId, textId, "Documents", "Commentary", commentary);
            if (result == 0)
            {
                return StatusCode(400);
            }
            try
            {
                return (await Service.GetCorpusById(CorpusId)).SingleOrDefault(x => x.Id == textId);
            }
            catch
            {
                return StatusCode(400);
            }
        }

        /// <summary>
        /// Добавляет текст аннотации к версии.
        /// </summary>
        /// <param name="CorpusId">Идентификатор документа.</param>
        /// <param name="textId">Идентификатор версии.</param>
        /// <param name="annotation">Текст аннотации.</param>
        /// <returns>Возвращает версию с добавленной аннотацией.</returns>
        /// <response code="200">Аннотация была добавлена успешно.</response>
        /// <response code="400">В процессе добавления возникли проблемы.</response>
        [HttpPatch]
        [Route("{textId}/Annotation")]
        public async Task<ActionResult<TextFileModel>> ChangeAnnotation([FromRoute] string CorpusId, [FromRoute] string textId, [FromBody] string annotation)
        {
            var result = await Service.EditTextCorpus(CorpusId, textId, "Documents", "Annotation", annotation);
            if (result == 0)
            {
                return StatusCode(400);
            }
            try
            {
                return (await Service.GetCorpusById(CorpusId)).SingleOrDefault(x => x.Id == textId);
            }
            catch
            {
                return StatusCode(400);
            }
        }
    }
}
