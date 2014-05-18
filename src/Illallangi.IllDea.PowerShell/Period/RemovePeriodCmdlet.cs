namespace Illallangi.IllDea.PowerShell.Period
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Remove, Nouns.Period, SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    public sealed class RemovePeriodCmdlet : GetPeriodCmdlet
    {
        protected override void ProcessRecord()
        {
            foreach (var period in this.Client.Period.Retrieve(this.CompanyId).Where(this.IsMatch))
            {
                this.Client.Period.Delete(
                    this.CompanyId,
                    period,
                    this.ToString());
            }
        }

        protected override bool IsMatch(IPeriod period)
        {
            return base.IsMatch(period) &&
                   this.ShouldProcess(period.ToString(), VerbsCommon.Remove);
        }

        public override string ToString()
        {
            return string.Format(
                @"Remove-Period -Id ""{0}""",
                this.Id);
        }
    }
}