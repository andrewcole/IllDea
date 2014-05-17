namespace Illallangi.IllDea.PowerShell.Txn
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Add, Nouns.TxnItem, DefaultParameterSetName = AddTxnItemCmdlet.NameParameterSet)]
    public sealed class AddTxnItemCmdlet : DeaCmdlet, ITxn
    {
        #region Fields

        private IList<TxnItem> currentItems;

        private string currentAccountName;

        private WildcardPattern currentAccountNameWildcardPattern;

        private Guid currentAccountId;

        private string currentAccountNumber;

        private WildcardPattern currentAccountNumberWildcardPattern;

        private const string IdParameterSet = @"Id";

        private const string NameParameterSet = @"Name";

        private const string AccountParameterSet = @"Account";

        private const string NumberParameterSet = @"Number";
        #endregion

        #region Properties

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.AccountParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NameParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NumberParameterSet)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.AccountParameterSet)]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NameParameterSet)]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NumberParameterSet)]
        public Guid Period { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.AccountParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NameParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NumberParameterSet)]
        public DateTime Date { get; set; }

        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.AccountParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NameParameterSet)]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NumberParameterSet)]
        public string Description { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.AccountParameterSet)]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NameParameterSet)]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = AddTxnItemCmdlet.NumberParameterSet)]
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

        [Parameter(Mandatory = false, ParameterSetName = AddTxnItemCmdlet.AccountParameterSet)]
        [Parameter(Mandatory = false, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = false, ParameterSetName = AddTxnItemCmdlet.NameParameterSet)]
        [Parameter(Mandatory = false, ParameterSetName = AddTxnItemCmdlet.NumberParameterSet)]
        public string Comment { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.AccountParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.NameParameterSet)]
        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.NumberParameterSet)]
        public double Amount { get; set; }

        
        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.AccountParameterSet)]
        public IAccount Account { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.IdParameterSet)]
        public Guid AccountId
        {
            get
            {
                switch (this.ParameterSetName)
                {
                    case AddTxnItemCmdlet.AccountParameterSet:
                        return this.Account.Id;

                    case AddTxnItemCmdlet.IdParameterSet:
                        return this.currentAccountId;

                    case AddTxnItemCmdlet.NameParameterSet:
                    case AddTxnItemCmdlet.NumberParameterSet:
                        return this.Client.Account.Retrieve(this.CompanyId).Single(this.IsMatch).Id;

                    default:
                        throw new NotImplementedException(this.ParameterSetName);
                }
            }
            set
            {
                this.currentAccountId = value;
            }
        }

        [SupportsWildcards]
        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.NameParameterSet)]
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
        [Parameter(Mandatory = true, ParameterSetName = AddTxnItemCmdlet.NumberParameterSet)]
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

        #region Methods

        protected override void ProcessRecord()
        {
            var txnItem = this.Items.SingleOrDefault(i => i.Account.Equals(this.AccountId));

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
                            Account = this.AccountId,
                            Amount = this.Amount,
                        });
            }
            this.WriteObject(this);
        }
        
        private bool IsMatch(IAccount account)
        {
            switch (this.ParameterSetName)
            {
                case NameParameterSet:
                    return this.AccountNameWildcardPattern.IsMatch(account.Name);

                case NumberParameterSet:
                    return this.AccountNumberWildcardPattern.IsMatch(account.Number);

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