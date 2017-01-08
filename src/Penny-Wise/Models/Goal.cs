using System;

namespace Penny_Wise.Models
{
    public class Goal
    {
        public int ID { get; set; }
        public double Amount { get; set; }
        public DateTime DateTo { get; set; }
        public UserAccount Account { get; set; }
    }
}
