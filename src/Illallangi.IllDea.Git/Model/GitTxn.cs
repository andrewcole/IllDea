namespace Illallangi.IllDea.Model
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public sealed class GitTxn : BaseModel, ITxn
    {
        #region Fields

        private IList<TxnItem> currentItems;

        #endregion

        #region Methods

        public bool ShouldSerializeAccounts()
        {
            return this.Items.Count > 0;
        }

        public bool ShouldSerializeInternal()
        {
            return this.Internal;
        }

        #endregion

        #region Properties

        [JsonProperty("internal")]
        public bool Internal { get; set; }

        [JsonProperty("period")]
        public Guid Period { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("items")]
        public IList<TxnItem> Items
        {
            get
            {
                return this.currentItems ?? (this.currentItems = new List<TxnItem>());
            }
        }

        #endregion

    }
}