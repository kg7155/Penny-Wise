using System;
using System.ComponentModel.DataAnnotations;

namespace Penny_Wise.Models
{
    public class Goal
    {
        public int ID { get; set; }
        public double Amount { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime DateTo { get; set; }
        public UserAccount Account { get; set; }
    }
}
