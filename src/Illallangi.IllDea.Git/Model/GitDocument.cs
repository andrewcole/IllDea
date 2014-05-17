namespace Illallangi.IllDea.Model
{
    using System;

    using Newtonsoft.Json;

    public sealed class GitDocument : BaseModel, IDocument
    {
        #region Properties

        [JsonProperty("period")]
        public Guid Period { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("compilation")]
        public bool Compilation { get; set; }

        [JsonIgnore]
        public Uri Uri { get; set; }
        
        #endregion
    }
}