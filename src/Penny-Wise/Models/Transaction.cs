using System;
using System.ComponentModel.DataAnnotations;

namespace Penny_Wise.Models
{
    public class Transaction
    {
        public int ID { get; set; }

        public bool Type { get; set; }

        public double Value { get; set; }

        public Category Category { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public UserAccount Account { get; set; }
    }
}
