namespace Illallangi.IllDea.PowerShell.Txn
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Set, Nouns.Txn)]
    public sealed class SetTxnCmdlet : DeaCmdlet, ITxn
    {
        private IList<TxnItem> currentAccounts;

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        public Guid Period { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public DateTime Date { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
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
            this.WriteObject(this.Client.Txn.Update(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"Set-Txn -Id ""{0}"" -Date ""{1}"" -Description ""{2}"" -Accounts ""{3}""",
                this.Id,
                this.Date,
                this.Description,
                this.Items);
        }
    }
}