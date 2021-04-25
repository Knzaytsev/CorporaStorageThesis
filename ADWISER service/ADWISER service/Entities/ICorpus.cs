using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADWISER_service.Models
{
    public interface ICorpus
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
