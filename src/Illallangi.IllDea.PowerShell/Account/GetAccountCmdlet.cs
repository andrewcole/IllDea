using System;

namespace Illallangi.IllDea.PowerShell.Account
{
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Get, Nouns.Account)]
    public class GetAccountCmdlet : DeaCmdlet
    {
        private const string FilterParameterSet = @"Filter";
        private const string IdParameterSet = @"Id";

        private string currentName;

        private string currentNumber;

        private WildcardPattern currentNumberWildcardPattern;
        
        private WildcardPattern currentNameWildcardPattern;

        [Parameter(Mandatory = true, ParameterSetName = GetAccountCmdlet.IdParameterSet, ValueFromPipelineByPropertyName = true)]
        public Guid? Id { get; set; }

        [SupportsWildcards]
        [Parameter(Position = 1, ParameterSetName = GetAccountCmdlet.FilterParameterSet)]
        public string Name
        {
            get
            {
                return this.currentName;
            }
            set
            {
                this.currentNameWildcardPattern = null; 
                this.currentName = value;
            }
        }

        [Parameter(ParameterSetName = GetAccountCmdlet.FilterParameterSet)]
        public AccountType? Type { get; set; }

        [SupportsWildcards]
        [Parameter(ParameterSetName = GetAccountCmdlet.FilterParameterSet)]
        public string Number
        {
            get
            {
                return this.currentNumber;
            }
            set
            {
                this.currentNumberWildcardPattern = null;
                this.currentNumber = value;
            }
        }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Account.Retrieve(this.CompanyId).Where(this.IsMatch), true);
        }

        protected virtual bool IsMatch(IAccount account)
        {
            switch (this.ParameterSetName)
            {
                case GetAccountCmdlet.IdParameterSet:
                    return this.Id.HasValue &&
                           this.Id.Value.Equals(account.Id);

                case GetAccountCmdlet.FilterParameterSet:
                    return this.NumberWildcardPattern.IsMatch(account.Number)
                           && (!this.Type.HasValue || account.Type.Equals(this.Type.Value))
                           && this.NameWildcardPattern.IsMatch(account.Name);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
            }
        }

        private WildcardPattern NumberWildcardPattern 
        {
            get
            {
                return this.currentNumberWildcardPattern
                       ?? (this.currentNumberWildcardPattern =
                           new WildcardPattern(this.Number ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        private WildcardPattern NameWildcardPattern
        {
            get
            {
                return this.currentNameWildcardPattern
                       ?? (this.currentNameWildcardPattern =
                           new WildcardPattern(this.Name ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }
    }
}
