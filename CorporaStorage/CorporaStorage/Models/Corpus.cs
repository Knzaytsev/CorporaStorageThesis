using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorporaStorage.Models
{
    public class Corpus : ICorpus
    {
        public string Name { get; set; }

        public IEnumerable<TextFile> Documents { get; set; }

        public string Author { get; set; }

        public DateTime Date { get; set; }
    }
}
