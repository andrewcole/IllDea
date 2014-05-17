namespace Illallangi.IllDea.PowerShell.Account
{
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Get, Nouns.Account)]
    public sealed class GetAccountCmdlet : DeaCmdlet
    {
        private string currentName;

        private string currentNumber;

        private WildcardPattern currentNumberWildcardPattern;
        
        private WildcardPattern currentNameWildcardPattern;

        [SupportsWildcards]
        [Parameter(Position = 1)]
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

        [Parameter]
        public AccountType? Type { get; set; }

        [SupportsWildcards]
        [Parameter]
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

        private bool IsMatch(IAccount account)
        {
            return this.NumberWildcardPattern.IsMatch(account.Number)
                   && (!this.Type.HasValue || account.Type.Equals(this.Type.Value))
                   && this.NameWildcardPattern.IsMatch(account.Name);
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
