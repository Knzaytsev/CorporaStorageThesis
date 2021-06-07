using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Models
{
    public class TextFileModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }

        public DateTime Date { get; set; }

        public string Annotation { get; set; }

        public int Mark { get; set; }

        public string Commentary { get; set; }
    }
}
