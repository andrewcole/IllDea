namespace Illallangi.IllDea.PowerShell.Employee
{
    using System;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Set, Nouns.Employee)]
    public sealed class SetEmployeeCmdlet : DeaCmdlet, IEmployee
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }
        
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Address { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string City { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string State { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string PostCode { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public Guid SalaryExpenseAccount { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public Guid EmployeeLiabilityAccount { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public Guid IncomeTaxLiabilityAccount { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public Guid SuperannuationExpenseAccount { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public Guid SuperannuationLiabilityAccount { get; set; }
        
        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Employee.Update(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"Set-Employee -Id ""{0}"" -Name ""{1}""",
                this.Id,
                this.Name);
        }
    }
}