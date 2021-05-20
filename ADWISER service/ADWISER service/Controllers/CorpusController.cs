using ADWISER_service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Controllers
{
    public class CorpusController : Controller
    {
        private IStorageService Service { get; set; }

        public CorpusController(IStorageService service)
        {
            Service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name, string Author)
        {
            await Service.AddCorpusToCollection(Author, Name);
            return Redirect("Main");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        [Route("ADWISER/{controller}/Details/{id}")]
        public async Task<IActionResult> Details([FromRoute] string id)
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
            return View();
        }

        [HttpGet]
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
        [Route("ADWISER/{controller}/Delete/{id}")]
        public async Task<IActionResult> Delete(string id)
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

        [HttpPost, ActionName("Delete")]
        [Route("ADWISER/{controller}/Delete/{id}")]
        public async Task<IActionResult> DeleteCorpusConfirmed(string id)
        {
            var result = await Service.RemoveCorpusFromCollection(id);
            if (result == 0)
            {
                return StatusCode(400);
            }
            return Redirect("/ADWISER/Corpus/Main");
        }
    }
}
