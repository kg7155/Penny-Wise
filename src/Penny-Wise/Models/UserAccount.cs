using System.Collections.Generic;

namespace Penny_Wise.Models
{
    public class UserAccount
    {
        public int ID { get; set; }

        public string Type { get; set; }

        public double Balance { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public List<Element> Elements { get; set; }
    }
}
