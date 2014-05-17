namespace Illallangi.IllDea.PowerShell.Period
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Remove, Nouns.Period)]
    public sealed class RemovePeriodCmdlet : DeaCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        protected override void ProcessRecord()
        {
            this.Client.Period.Delete(
                this.CompanyId,
                this.Client.Period.Retrieve(this.CompanyId).Single(p => p.Id.Equals(this.Id)),
                this.ToString());
        }

        public override string ToString()
        {
            return string.Format(
                @"Remove-Period -Id ""{0}""",
                this.Id);
        }
    }
}