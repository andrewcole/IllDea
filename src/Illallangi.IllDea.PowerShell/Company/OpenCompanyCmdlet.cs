namespace Illallangi.IllDea.PowerShell.Company
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Open, Nouns.Company, DefaultParameterSetName = OpenCompanyCmdlet.NameParameterSet)]
    public sealed class OpenCompanyCmdlet : DeaCmdlet
    {
        private string currentName;

        private WildcardPattern currentNameWildcardPattern;

        public const string IdParameterSet = @"Id";

        public const string NameParameterSet = @"Name";

        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = OpenCompanyCmdlet.IdParameterSet)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false, Position = 1, ParameterSetName = OpenCompanyCmdlet.NameParameterSet)]
        public string Name
        {
            get
            {
                return this.currentName;
            }
            set
            {
                this.currentName = value;
                this.currentNameWildcardPattern = null;
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

        private ICompany GetCompany()
        {
            switch (this.ParameterSetName)
            {
                case OpenCompanyCmdlet.IdParameterSet:
                    return this.Client.Company.Retrieve().Single(c => c.Id.Equals(this.Id));

                case OpenCompanyCmdlet.NameParameterSet:
                    return this.Client.Company.Retrieve().Single(this.IsMatch);

                default:
                    throw new PSNotImplementedException(this.ParameterSetName);
            }
        }

        private bool IsMatch(ICompany company)
        {
            return this.NameWildcardPattern.IsMatch(company.Name);
        }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.OpenCompany(this.GetCompany()));
        }
    }
}