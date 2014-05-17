namespace Illallangi.IllDea.Model
{
    using Newtonsoft.Json;

    public sealed class GitCompany : BaseModel, ICompany
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
