namespace Illallangi.IllDea.Model
{
    using System;

    public class TxnItem
    {
        public string Comment { get; set; }

        public Guid Account { get; set; }

        public decimal Amount { get; set; }

        public bool IsDebit 
        {
            get
            {
                return (this.Amount < 0);
            }
        }
    }
}