namespace Illallangi.IllDea.PowerShell.Txn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Add, Nouns.TxnItem)]
    public sealed class AddTxnItemCmdlet : DeaCmdlet, ITxn
    {
        #region Fields

        private const string FilterParameterSet = @"Filter";
        private const string IdParameterSet = @"Id";

        private IList<TxnItem> currentItems;

        private string currentAccountName;

        private WildcardPattern currentAccountNameWildcardPattern;

        private Guid currentAccountId;

        private string currentAccountNumber;

        private WildcardPattern currentAccountNumberWildcardPattern;

        #endregion

        #region Properties

        #region Txn Properties

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.FilterParameterSet)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.FilterParameterSet)]
        public Guid Period { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.FilterParameterSet)]
        public DateTime Date { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.FilterParameterSet)]
        public string Description { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.FilterParameterSet)]
        public IList<TxnItem> Items
        {
            get
            {
                return this.currentItems ?? (this.currentItems = new List<TxnItem>());
            }
            set
            {
                this.currentItems = value;
            }
        }

        #endregion

        #region TxnItem Properties

        [Parameter(Mandatory = false, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = false, ParameterSetName = AddTxnItemCmdlet.FilterParameterSet)]
        public string Comment { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.FilterParameterSet)]
        public double Amount { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        public Guid? AccountId { get; set; }
        
        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = AddTxnItemCmdlet.FilterParameterSet)]
        public string AccountName
        {
            get
            {
                return this.currentAccountName;
            }
            set
            {
                this.currentAccountName = value;
                this.currentAccountNameWildcardPattern = null;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = AddTxnItemCmdlet.FilterParameterSet)]
        public string AccountNumber
        {
            get
            {
                return this.currentAccountNumber;
            }
            set
            {
                this.currentAccountNumber = value;
                this.currentAccountNumberWildcardPattern = null;
            }
        }

        private WildcardPattern AccountNameWildcardPattern
        {
            get
            {
                return this.currentAccountNameWildcardPattern
                       ?? (this.currentAccountNameWildcardPattern =
                           new WildcardPattern(this.AccountName ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern AccountNumberWildcardPattern
        {
            get
            {
                return this.currentAccountNumberWildcardPattern
                       ?? (this.currentAccountNumberWildcardPattern =
                           new WildcardPattern(this.AccountNumber ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        #endregion

        #endregion

        #region Methods

        protected override void ProcessRecord()
        {
            var account = this.Client.Account.Retrieve(this.CompanyId).Single(this.IsMatch);

            var txnItem = this.Items.SingleOrDefault(i => i.Account.Equals(account.Id));

            if (null != txnItem)
            {
                txnItem.Amount += this.Amount;
                txnItem.Comment = this.Comment ?? txnItem.Comment;
            }
            else
            {
                this.Items.Add(
                    new TxnItem
                        {
                            Comment = this.Comment ?? string.Empty,
                            Account = account.Id,
                            Amount = this.Amount,
                        });
            }
            this.WriteObject(this);
        }
        
        private bool IsMatch(IAccount account)
        {
            switch (this.ParameterSetName)
            {
                case AddTxnItemCmdlet.IdParameterSet:
                    return this.AccountId.HasValue &&
                           this.AccountId.Value.Equals(account.Id);

                case AddTxnItemCmdlet.FilterParameterSet:
                    return this.AccountNumberWildcardPattern.IsMatch(account.Number)
                           && this.AccountNameWildcardPattern.IsMatch(account.Name);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        public override string ToString()
        {
            return string.Format(
                @"Add-TxnItem -Id ""{0}"" -Date ""{1}"" -Description ""{2}"" -Accounts ""{3}""",
                this.Id,
                this.Date,
                this.Description,
                this.Items);
        }

        #endregion
    }
}