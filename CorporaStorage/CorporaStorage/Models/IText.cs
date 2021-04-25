using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorporaStorage.Models
{
    public interface IText : ICorpus
    {
        public string Annotation { get; set; }

        public string Source { get; set; }
    }
}
