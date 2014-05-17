namespace Illallangi.IllDea.PowerShell.Txn
{
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [OutputType(typeof(ITxn))]
    [Cmdlet(VerbsCommon.Get, Nouns.Txn)]
    public sealed class GetTxnCmdlet : DeaCmdlet
    {
        private WildcardPattern currentDescriptionWildcardPattern;

        private string currentDescription;

        [SupportsWildcards]
        [Parameter(Mandatory = false)]
        public string Description
        {
            get
            {
                return this.currentDescription;
            }
            set
            {
                this.currentDescription = value;
                this.currentDescriptionWildcardPattern = null;
            }
        }

        private WildcardPattern DescriptionWildcardPattern
        {
            get
            {
                return this.currentDescriptionWildcardPattern
                       ?? (this.currentDescriptionWildcardPattern =
                           new WildcardPattern(this.Description ?? @"*", WildcardOptions.Compiled | WildcardOptions.IgnoreCase));
            }
        }

        protected override void ProcessRecord()
        {
            this.WriteObject(
                this.Client.Txn.Retrieve(this.CompanyId).Where(this.IsMatch),
                true);
        }

        private bool IsMatch(ITxn txn)
        {
            return this.DescriptionWildcardPattern.IsMatch(txn.Description);
        }
    }
}
