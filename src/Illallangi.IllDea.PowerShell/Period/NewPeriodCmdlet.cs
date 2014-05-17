namespace Illallangi.IllDea.PowerShell.Period
{
    using System;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.New, Nouns.Period)]
    public sealed class NewPeriodCmdlet : IdDeaCmdlet, IPeriod
    {
        [Parameter(Mandatory = true)]
        public DateTime Start { get; set; }

        [Parameter(Mandatory = true)]
        public DateTime End { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Period.Create(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"New-Period -Start ""{0}"" -End ""{1}""",
                this.Start,
                this.End);
        }
    }
}