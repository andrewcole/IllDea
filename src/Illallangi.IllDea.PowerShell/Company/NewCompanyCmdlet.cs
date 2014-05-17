namespace Illallangi.IllDea.PowerShell.Company
{
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.New, Nouns.Company)]
    public sealed class NewCompanyCmdlet : IdDeaCmdlet, ICompany, ISettings
    {
        [Parameter(Mandatory = true, Position = 1)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("EMail")]
        public string AuthorEmail { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("Author")]
        public string AuthorName { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.OpenCompany(this.Client.Company.Create(this, this, this.ToString())));
        }

        public override string ToString()
        {
            return string.Format(
                @"New-Company -Name ""{0}"" -AuthorEmail ""{1}"" -AuthorName ""{2}""",
                this.Name,
                this.AuthorEmail,
                this.AuthorName);
        }
    }
}