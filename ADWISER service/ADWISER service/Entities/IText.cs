using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Models
{
    interface IText
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Annotation { get; set; }

        public string Source { get; set; }
    }
}
