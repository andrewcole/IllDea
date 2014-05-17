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

        #endregion
    }
}