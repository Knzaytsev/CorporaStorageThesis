using ADWISER_service.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Services
{
    public interface IStorageService
    {
        public Task<List<TextFileModel>> GetCorpusById(string id);

        public Task<CorpusModel> AddCorpusToCollection(string author, string name);

        public Task</*TextFileModel*/int> AddTextToCorpus(string id, string set, InputTextFileModel textModel);

        public Task<List<CorpusModel>> GetCorporaFromStorage();

        public Task</*CorpusModel*/int> RemoveTextFromCorpus(string set, string corpusId, string textId);

        public Task</*TextFileModel*/int> EditTextCorpus(string corpusId, string textId, string set, string field, string text);

        public Task<int> RemoveCorpusFromCollection(string corpusId);
    }
}
