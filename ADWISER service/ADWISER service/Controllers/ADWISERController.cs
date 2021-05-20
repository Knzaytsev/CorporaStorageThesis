using ADWISER_service.Models;
using ADWISER_service.Services;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            try
            {
                ViewBag.Texts = await Service.GetCorpusById(id);
            }
            catch
            {
                return StatusCode(400);
            }
            ViewBag.CorpusId = id;
            //return await Service.GetCorpusById(CorpusId);
            return View();
        }

        [HttpGet]
        [Route("ADWISER/CorpusView/EditTextFile/{CorpusId}/{TextId}")]
        public async Task<IActionResult> EditTextFile([FromRoute] string CorpusId, [FromRoute] string TextId)
        {
            try
            {
                var corpus = await Service.GetCorpusById(CorpusId);
                var text = corpus.SingleOrDefault(x => x.Id == TextId);
                ViewBag.Text = text;
                ViewBag.CorpusId = CorpusId;
            }
            catch
            {
                return StatusCode(400);
            }
            
            return View();
        }

        [HttpPost]
        [Route("ADWISER/CorpusView/EditTextFile/{CorpusId}/{TextId}")]
        public async Task<IActionResult> EditTextFile([FromRoute] string corpusId, [FromRoute] string textId, string source, string annotation)
        {
            var result = await Service.EditTextCorpus(corpusId, textId, "Documents", "Source", source);
            if (result == 0)
            {
                return StatusCode(400);
            }
            result = await Service.EditTextCorpus(corpusId, textId, "Documents", "Annotation", annotation);
            if (result == 0)
            {
                return StatusCode(400);
            }
            ViewBag.CorpusId = corpusId;
            try
            {
                var corpus = await Service.GetCorpusById(corpusId);
                var text = corpus.SingleOrDefault(x => x.Id == textId);
                ViewBag.Text = text;
            }
            catch
            {
                return StatusCode(400);
            }
            
            return View();
        }

        [HttpPost]
        //[Route("documents/{id}")]
        [Route("ADWISER/AddText/{id}")]
        public async Task<IActionResult> AddText(string CorpusId, InputTextFileModel text/*[FromRoute] string id, [FromBody] InputTextFileModel text, [FromQuery] string set*/)
        {
            var result = await Service.AddTextToCorpus(CorpusId, "Documents", text/*id, set, text*/);
            if (result == 0)
            {
                return StatusCode(400);
            }
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
            try
            {
                ViewBag.Corpora = await Service.GetCorporaFromStorage();
            }
            catch
            {
                return StatusCode(400);
            }
            
            return View();
        }

        [HttpGet]
        [Route("ADWISER/DeleteCorpus/{id}")]
        public async Task<IActionResult> DeleteCorpus(string id)
        {
            try
            {
                ViewBag.Texts = await Service.GetCorpusById(id);
            }
            catch
            {
                return StatusCode(400);
            }
            
            return View();
        }

        [HttpPost, ActionName("DeleteCorpus")]
        [Route("ADWISER/DeleteCorpus/{id}")]
        public async Task<IActionResult> DeleteCorpusConfirmed(string id)
        {
            var result = await Service.RemoveCorpusFromCollection(id);  
            if (result == 0)
            {
                return StatusCode(400);
            }
            return Redirect("/ADWISER/Main");
        }

        [HttpGet]
        [Route("ADWISER/CorpusView/RemoveText/{corpusId}/{textId}")]
        public async Task<IActionResult> RemoveText([FromRoute] string corpusId, [FromRoute] string textId)
        {
            try
            {
                var corpus = await Service.GetCorpusById(corpusId);
                var text = corpus.SingleOrDefault(x => x.Id == textId);
                ViewBag.Text = text;
                ViewBag.CorpusId = corpusId;
            }
            catch
            {
                return StatusCode(400);
            }
            
            return View();
        }

        [HttpPost, ActionName("RemoveText")]
        [Route("ADWISER/CorpusView/RemoveText/{corpusId}/{textId}")]
        public async Task<IActionResult> RemoveTextConfirmed([FromRoute] string corpusId, [FromRoute] string textId)
        {
            var result = await Service.RemoveTextFromCorpus("Documents", corpusId, textId);
            if(result == 0)
            {
                return StatusCode(400);
            }
            return Redirect($"/ADWISER/CorpusView/{corpusId}");
        }

        [HttpGet]
        [Route("ADWISER/UploadText/{id}")]
        public async Task<ActionResult> UploadText(string id)
        {
            await Service.GetCorporaFromStorage();
            return View();
        }

        [HttpPost]
        [Route("ADWISER/UploadText/{id}")]
        public async Task<ActionResult> UploadText(string id, IFormFile upload)
        {
            //var upload = HttpContext.Request.Form.Files[0];
            if (upload != null)
            {
                // получаем имя файла
                string fileName = System.IO.Path.GetFileName(upload.FileName);

                var stream = upload.OpenReadStream();
                var buffer = new byte[upload.Length];
                stream.Read(buffer);
                var result = Encoding.UTF8.GetString(buffer);
                var model = new InputTextFileModel
                {
                    Name = fileName,
                    Source = result,
                    Annotation = ""
                };
                var response = await Service.AddTextToCorpus(id, "Documents", model);
                if (response == 0)
                {
                    return StatusCode(400);
                }
            }
            return Redirect($"/ADWISER/CorpusView/{id}");
        }
    }
}
