namespace Illallangi.IllDea.PowerShell.Company
{
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Set, Nouns.Company)]
    public sealed class SetCompanyCmdlet : IdDeaCmdlet, ICompany
    {
        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Company.Update(this.CompanyId, this, this.ToString()));
        }

        [Parameter(Mandatory = true, Position = 1)]
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format(
                @"Set-Company -Name ""{0}""",
                this.Name);
        }
    }
}