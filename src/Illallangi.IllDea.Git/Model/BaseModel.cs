namespace Illallangi.IllDea.Model
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    public class BaseModel : IBaseModel
    {
        private string currentClass;

        [JsonProperty("self")]
        [JsonConverter(typeof(HrefConverter))]
        public Guid Id { get; set; }

        [JsonProperty("index")]
        public Guid Index { get; set; }

        [JsonProperty("class")]
        public string Class
        {
            get
            {
                return this.currentClass ?? (this.currentClass = this.GetType().Name.ToLower().TrimStart("git"));
            }

            set
            {
                if (value != this.Class)
                {
                    throw new InvalidDataException(string.Format(@"Class ""{0}"" encountered in json, expected ""{1}""", value, this.Class));
                }
            }
        }
    }
}