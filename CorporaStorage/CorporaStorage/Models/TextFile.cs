using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorporaStorage.Models
{
    public class TextFile : IText
    {
        public string Name { get; set; }

        public string Source { get; set; }

        public DateTime Date { get; set; }

        public string Annotation { get; set; }
    }
}
