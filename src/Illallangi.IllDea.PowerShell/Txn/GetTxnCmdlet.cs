namespace Illallangi.IllDea.PowerShell.Txn
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [OutputType(typeof(ITxn))]
    [Cmdlet(VerbsCommon.Get, Nouns.Txn)]
    public class GetTxnCmdlet : DeaCmdlet
    {
        private const string FilterParameterSet = @"Filter";
        private const string IdParameterSet = @"Id";

        private string currentDescription;

        private WildcardPattern currentDescriptionWildcardPattern;

        [Parameter(Mandatory = true, ParameterSetName = GetTxnCmdlet.IdParameterSet, ValueFromPipelineByPropertyName = true)]
        public Guid? Id { get; set; }

        [Parameter(Position = 1, ParameterSetName = GetTxnCmdlet.FilterParameterSet)]
        public DateTime? Date { get; set; }

        [SupportsWildcards]
        [Parameter(Position = 1, ParameterSetName = GetTxnCmdlet.FilterParameterSet)]
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

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Txn.Retrieve(this.CompanyId).Where(this.IsMatch), true);
        }

        protected virtual bool IsMatch(ITxn txn)
        {
            switch (this.ParameterSetName)
            {
                case GetTxnCmdlet.IdParameterSet:
                    return this.Id.HasValue &&
                           this.Id.Value.Equals(txn.Id);

                case GetTxnCmdlet.FilterParameterSet:
                    return this.DescriptionWildcardPattern.IsMatch(txn.Description)
                           && (!this.Date.HasValue || txn.Date.Equals(this.Date.Value));

                default:
                    throw new NotImplementedException(this.ParameterSetName);
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
    }
}
