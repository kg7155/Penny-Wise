using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Penny_Wise.Models
{
    public class Transaction
    {
        public int ID { get; set; }

        public bool Type { get; set; }

        public double Value { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public UserAccount Account { get; set; }

        public List<CategoryTransaction> CategoryTransactions { get; set; }
    }
}
