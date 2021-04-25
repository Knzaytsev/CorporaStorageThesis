using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorporaStorage.Models
{
    public interface ICorpus
    {
        public string Name { get; set; }

        public DateTime Date { get; set; }
    }
}
