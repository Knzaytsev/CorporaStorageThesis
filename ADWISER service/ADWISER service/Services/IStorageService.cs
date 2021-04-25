using ADWISER_service.Models;
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

        public Task<TextFileModel> AddTextToCorpus(string id, string set, InputTextFileModel textModel/*string name, string source, string annotation*/);

        public Task<List<CorpusModel>> GetCorporaFromStorage();

        public Task<CorpusModel> RemoveTextFromCorpus(string set, string corpusId, string textId);

        public Task<TextFileModel> EditTextCorpus(string corpusId, string textId, string set, string field, string text);
    }
}
