using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Penny_Wise.Models
{
    public class CategoryElement
    {
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public int ElementID { get; set; }
        public Element Element { get; set; }
    }
}
