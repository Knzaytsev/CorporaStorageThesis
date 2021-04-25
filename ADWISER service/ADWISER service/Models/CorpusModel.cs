using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Models
{
    public class CorpusModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<TextFileModel> Documents { get; set; }

        public string Author { get; set; }

        public DateTime Date { get; set; }
    }
}
