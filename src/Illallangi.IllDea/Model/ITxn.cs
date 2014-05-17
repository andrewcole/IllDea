namespace Illallangi.IllDea.Model
{
    using System;
    using System.Collections.Generic;

    public interface ITxn : IBaseModel
    {
        Guid Period { get; set; }

        DateTime Date { get; set; }

        string Description { get; set; }

        IList<TxnItem> Items { get; }
    }
}