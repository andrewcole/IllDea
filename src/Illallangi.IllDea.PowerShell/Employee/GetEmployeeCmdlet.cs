using System;

namespace Illallangi.IllDea.PowerShell.Employee
{
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Get, Nouns.Employee)]
    public class GetEmployeeCmdlet : DeaCmdlet
    {
        private const string FilterParameterSet = @"Filter";
        private const string IdParameterSet = @"Id";

        private string currentName;

        private WildcardPattern currentNameWildcardPattern;

        [Parameter(Mandatory = true, ParameterSetName = GetEmployeeCmdlet.IdParameterSet, ValueFromPipelineByPropertyName = true)]
        public Guid? Id { get; set; }

        [SupportsWildcards]
        [Parameter(Position = 1, ParameterSetName = GetEmployeeCmdlet.FilterParameterSet)]
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

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Employee.Retrieve(this.CompanyId).Where(this.IsMatch), true);
        }

        protected virtual bool IsMatch(IEmployee employee)
        {
            switch (this.ParameterSetName)
            {
                case GetEmployeeCmdlet.IdParameterSet:
                    return this.Id.HasValue &&
                           this.Id.Value.Equals(employee.Id);

                case GetEmployeeCmdlet.FilterParameterSet:
                    return this.NameWildcardPattern.IsMatch(employee.Name);

                default:
                    throw new NotImplementedException(this.ParameterSetName);
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
