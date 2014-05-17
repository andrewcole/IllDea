namespace Illallangi.IllDea.PowerShell.Period
{
    using System;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Set, Nouns.Period)]
    public sealed class SetPeriodCmdlet : DeaCmdlet, IPeriod
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public DateTime Start { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public DateTime End { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Period.Update(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"Set-Period -Id ""{0}"" -Start ""{1}"" -End ""{2}""",
                this.Id,
                this.Start,
                this.End);
        }
    }
}