using System.Collections.Generic;

namespace Penny_Wise.Models
{
    public class Category
    {
        public int ID { get; set; }
        
        public string Name { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
