using System;

namespace Illallangi.IllDea.Model
{
    using Newtonsoft.Json;

    public sealed class GitEmployee : BaseModel, IEmployee
    {
        #region Properties
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("postcode")]
        public string PostCode { get; set; }

        [JsonProperty("salaryexpense")]
        public Guid SalaryExpenseAccount { get; set; }

        [JsonProperty("employeeliability")]
        public Guid EmployeeLiabilityAccount { get; set; }

        [JsonProperty("taxliability")]
        public Guid IncomeTaxLiabilityAccount { get; set; }

        [JsonProperty("superexpense")]
        public Guid SuperannuationExpenseAccount { get; set; }

        [JsonProperty("superliability")]
        public Guid SuperannuationLiabilityAccount { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}