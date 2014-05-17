namespace Illallangi.IllDea.PowerShell.Settings
{
    using System.Management.Automation;

    [Cmdlet(VerbsCommon.Get, Nouns.Settings)]
    public sealed class GetSettingsCmdlet : DeaCmdlet
    {
        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Settings.Retrieve(this.CompanyId), true);
        }
    }
}
