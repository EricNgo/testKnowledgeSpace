using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSpace.ViewModels.System
{
    public class Pagination<T>
    {
        public List<T> Items { get; set; }

        public int PageCount { get; set; }
            
        
    }
}
