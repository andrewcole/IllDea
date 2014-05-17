namespace Illallangi.IllDea.PowerShell.Company
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Get, Nouns.Company)]
    public sealed class GetCompanyCmdlet : DeaCmdlet
    {
        private string currentName;

        private WildcardPattern currentNameWildcardPattern;

        private const string IdParameterSet = @"Id";

        private const string NameParameterSet = @"Name";

        [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = GetCompanyCmdlet.IdParameterSet)]
        public Guid Id { get; set; }

        [SupportsWildcards]
        [Parameter(Mandatory = false, ParameterSetName = GetCompanyCmdlet.NameParameterSet)]
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

        private IEnumerable<ICompany> GetCompanies()
        {
            switch (this.ParameterSetName)
            {
                case GetCompanyCmdlet.IdParameterSet:
                    return this.Client.Company.Retrieve().Where(c => c.Id.Equals(this.Id));

                case GetCompanyCmdlet.NameParameterSet:
                    return this.Client.Company.Retrieve().Where(this.IsMatch);

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
            this.WriteObject(this.GetCompanies(), true);
        }
    }
}
