namespace Illallangi.IllDea.Model
{
    using Newtonsoft.Json;

    public sealed class GitAccount : BaseModel, IAccount
    {
        #region Properties
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public AccountType Type { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonIgnore]
        public decimal Opening { get; set; }

        [JsonIgnore]
        public decimal Closing { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format(@"{2} Account #{0}: {1}",
                this.Number,
                this.Name,
                this.Type);
        }

        #endregion
    }
}