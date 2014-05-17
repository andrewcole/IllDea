namespace Illallangi.IllDea.PowerShell.Txn
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.New, Nouns.Txn)]
    public sealed class NewTxnCmdlet : IdDeaCmdlet, ITxn
    {
        private IList<TxnItem> currentAccounts;

        [Parameter(Mandatory = true)]
        public DateTime Date { get; set; }

        public Guid Period { get; set; }

        [Parameter(Mandatory = true)]
        public string Description { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public IList<TxnItem> Items
        {
            get
            {
                return this.currentAccounts ?? (this.currentAccounts = new List<TxnItem>());
            }
            set
            {
                this.currentAccounts = value;
            }
        }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Txn.Create(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"New-Txn -Date ""{0}"" -Description ""{1}"" -Items ""{2}""",
                this.Date,
                this.Description,
                this.Items);
        }
    }
}