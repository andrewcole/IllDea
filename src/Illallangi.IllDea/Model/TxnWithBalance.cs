using System;
using System.Collections.Generic;

namespace Illallangi.IllDea.Model
{
    public sealed class TxnWithBalance : ITxn
    {
        private IList<TxnItem> currentItems;
        public Guid Id { get; set; }
        public Guid Period { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public IList<TxnItem> Items
        {
            get
            {
                return this.currentItems ?? (this.currentItems = new List<TxnItem>());
            }

            set { this.currentItems = value; }
        }

        public decimal Balance { get; set; }
    }
}