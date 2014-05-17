namespace Illallangi.IllDea.PowerShell.Period
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Get, Nouns.Period)]
    public sealed class GetPeriodCmdlet : DeaCmdlet
    {
        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Period.Retrieve(this.CompanyId), true);
        }
    }
}
