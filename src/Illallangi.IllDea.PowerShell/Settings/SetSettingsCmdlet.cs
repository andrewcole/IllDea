namespace Illallangi.IllDea.PowerShell.Settings
{
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Set, Nouns.Settings)]
    public sealed class SetSettingsCmdlet : IdDeaCmdlet, ISettings
    {
        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Settings.Update(this.CompanyId, this));
        }

        [Parameter(Mandatory = false)]
        [Alias("EMail")]
        public string AuthorEmail { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("Author")]
        public string AuthorName { get; set; }
    }
}