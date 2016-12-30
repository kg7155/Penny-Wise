using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Penny_Wise.Models
{
    public class Element
    {
        public int ID { get; set; }

        public bool Type { get; set; }

        public double Value { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public UserAccount Account { get; set; }

        public List<CategoryElement> CategoryElements { get; set; }
    }
}
