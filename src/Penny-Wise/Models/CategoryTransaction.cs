namespace Penny_Wise.Models
{
    public class CategoryTransaction
    {
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public int TransactionID { get; set; }
        public Transaction Transaction { get; set; }
    }
}
