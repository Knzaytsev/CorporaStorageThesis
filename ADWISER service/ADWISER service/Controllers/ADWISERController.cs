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
    //[ApiController]
    //[Route("[controller]")]
    public class ADWISERController : Controller
    {
        private IStorageService Service { get; set; }

        public ADWISERController(IStorageService service)
        {
            Service = service;
        }

        [HttpPost]
        //[Route("documents/add")]
        public async Task<IActionResult> CreateDocument(string Name, string Author)
        {
            await Service.AddCorpusToCollection(Author, Name);
            return Redirect("Main");
        }

        [HttpGet]
        //[Route("documents/add")]
        public IActionResult CreateDocument()
        {
            return View();
        }

        [HttpGet]
        //[Route("documents/{id}")]
        [Route("ADWISER/CorpusView/{id}")]
        public async Task<IActionResult> CorpusView([FromRoute] string id)
        {
            ViewBag.Texts = await Service.GetCorpusById(id);
            ViewBag.CorpusId = id;
            //return await Service.GetCorpusById(CorpusId);
            return View();
        }

        [HttpGet]
        [Route("ADWISER/CorpusView/EditTextFile/{CorpusId}/{TextId}")]
        public async Task<IActionResult> EditTextFile([FromRoute] string CorpusId, [FromRoute] string TextId)
        {
            var corpus = await Service.GetCorpusById(CorpusId);
            var text = corpus.SingleOrDefault(x => x.Id == TextId);
            ViewBag.Text = text;
            ViewBag.CorpusId = CorpusId;
            return View();
        }

        [HttpPost]
        [Route("ADWISER/CorpusView/EditTextFile/{CorpusId}/{TextId}")]
        public async Task<IActionResult> EditTextFile([FromRoute] string corpusId, [FromRoute] string textId, string source, string annotation)
        {
            await Service.EditTextCorpus(corpusId, textId, "Documents", "Source", source);
            await Service.EditTextCorpus(corpusId, textId, "Documents", "Annotation", annotation);
            ViewBag.CorpusId = corpusId;
            var corpus = await Service.GetCorpusById(corpusId);
            var text = corpus.SingleOrDefault(x => x.Id == textId);
            ViewBag.Text = text;
            return View();
        }

        [HttpPost]
        //[Route("documents/{id}")]
        [Route("ADWISER/AddText/{id}")]
        public async Task<IActionResult> AddText(string CorpusId, InputTextFileModel text/*[FromRoute] string id, [FromBody] InputTextFileModel text, [FromQuery] string set*/)
        {
            var t = await Service.AddTextToCorpus(CorpusId, "Documents", text/*id, set, text*/);
            return Redirect($"/ADWISER/CorpusView/{CorpusId}");
            //return View();
        }

        [HttpGet]
        [Route("ADWISER/AddText/{id}")]
        public async Task<IActionResult> AddText(string id)
        {
            ViewBag.CorpusId = id;
            return View();
        }

        [HttpGet]
        //[Route("documents")]
        public async Task<IActionResult> Main()
        {
            ViewBag.Corpora = await Service.GetCorporaFromStorage();
            return View();
        }

        [HttpGet]
        [Route("ADWISER/DeleteCorpus/{id}")]
        public async Task<IActionResult> DeleteCorpus(string id)
        {
            return View();
        }

        [HttpPost, ActionName("ADWISER/DeleteCorpus/{id}")]
        [Route("ADWISER/DeleteCorpus/{id}")]
        public async Task<IActionResult> DeleteCorpusConfirmed(string id)
        {
            return View();
        }

        [HttpDelete]
        [Route("documents/{corpusId}/{textId}")]
        public async Task<object> RemoveText([FromRoute] string corpusId, [FromRoute] string textId, [FromQuery] string set)
        {
            return await Service.RemoveTextFromCorpus(set, corpusId, textId);
        }
    }
}
