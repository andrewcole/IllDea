namespace Illallangi.IllDea.Model
{
    using System;
    using System.Collections.Generic;
    using Illallangi.IllDea.Extensions;

    using Microsoft.Win32;

    using Newtonsoft.Json;

    public class GitSettings : BaseModel, ISettings
    {
        #region Fields

        private IList<Guid> currentAccounts;
        private IList<Guid> currentPeriods;
        private IList<Guid> currentTxns;
        private IList<Guid> currentDocuments;
        private IList<Guid> currentEmployees;
        private IList<Guid> currentPayrolls;
            #endregion

        #region Properties

        [JsonIgnore]
        public string AuthorName
        {
            get
            {
                return Registry.CurrentUser.GetValue(
                    string.Format(@"Software\Illallangi Enterprises\Double Entry Accounting\{0}", this.Index),
                    @"AuthorName",
                    string.Empty);
            }

            set
            {
                Registry.CurrentUser.SetValue(
                    string.Format(@"Software\Illallangi Enterprises\Double Entry Accounting\{0}", this.Index),
                    @"AuthorName",
                    value);
            }
        }


        [JsonIgnore]
        public string AuthorEmail
        {
            get
            {
                return Registry.CurrentUser.GetValue(
                    string.Format(@"Software\Illallangi Enterprises\Double Entry Accounting\{0}", this.Index),
                    @"AuthorEmail",
                    string.Empty);
            }

            set
            {
                Registry.CurrentUser.SetValue(
                    string.Format(@"Software\Illallangi Enterprises\Double Entry Accounting\{0}", this.Index),
                    @"AuthorEmail",
                    value);
            }
        }

        [JsonIgnore]
        public string RootPath { get; set; }

        [JsonProperty("company")]
        public Guid Company { get; set; }

        [JsonProperty("accounts")]
        public IList<Guid> Accounts
        {
            get
            {
                return this.currentAccounts ?? (this.currentAccounts = new List<Guid>());
            }
        }

        [JsonProperty("periods")]
        public IList<Guid> Periods
        {
            get
            {
                return this.currentPeriods ?? (this.currentPeriods = new List<Guid>());
            }
        }

        [JsonProperty("txns")]
        public IList<Guid> Txns
        {
            get
            {
                return this.currentTxns ?? (this.currentTxns = new List<Guid>());
            }
        }

        [JsonProperty("documents")]
        public IList<Guid> Documents
        {
            get
            {
                return this.currentDocuments ?? (this.currentDocuments = new List<Guid>());
            }
        }

        [JsonProperty("employees")]
        public IList<Guid> Employees
        {
            get
            {
                return this.currentEmployees ?? (this.currentEmployees = new List<Guid>());
            }
        }

        [JsonProperty("payrolls")]
        public IList<Guid> Payrolls
        {
            get
            {
                return this.currentPayrolls ?? (this.currentPayrolls = new List<Guid>());
            }
        }

        #endregion

        #region Methods

        public bool ShouldSerializeAccounts()
        {
            return this.Accounts.Count > 0;
        }

        public bool ShouldSerializePeriods()
        {
            return this.Periods.Count > 0;
        }

        public bool ShouldSerializeTxns()
        {
            return this.Txns.Count > 0;
        }

        public bool ShouldSerializeDocuments()
        {
            return this.Documents.Count > 0;
        }

        public bool ShouldSerializeEmployees()
        {
            return this.Employees.Count > 0;
        }

        #endregion
    }
}