using System;
using Newtonsoft.Json;

namespace Illallangi.IllDea.Model
{
    public class GitPayroll : BaseModel, IPayroll
    {
        [JsonProperty("start")]
        public DateTime Start { get; set; }

        [JsonProperty("end")]
        public DateTime End { get; set; }

        [JsonProperty("employee")]
        public Guid Employee { get; set; }

        [JsonProperty("paytxn")]
        public Guid PayTxn { get; set; }

        [JsonProperty("supertxn")]
        public Guid SuperTxn { get; set; }

        [JsonProperty("grosspay")]
        public decimal GrossPay { get; set; }

        [JsonProperty("tax")]
        public decimal Tax { get; set; }

        [JsonProperty("netpay")]
        public decimal NetPay { get; set; }

        [JsonProperty("super")]
        public decimal Super { get; set; }
    }
}