using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Models
{
    public class InputTextFileModel
    {
        public string Name { get; set; }

        public string Source { get; set; }

        public DateTime Date { get => DateTime.Now; }

        public string Annotation { get; set; }
    }
}
