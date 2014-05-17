namespace Illallangi.IllDea.Model
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public sealed class GitPeriod : BaseModel, IPeriod
    {
        #region Properties

        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }

        #endregion
    }
}