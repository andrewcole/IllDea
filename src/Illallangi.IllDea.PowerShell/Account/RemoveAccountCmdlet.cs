namespace Illallangi.IllDea.PowerShell.Account
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Remove, Nouns.Account)]
    public sealed class RemoveAccountCmdlet : DeaCmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        protected override void ProcessRecord()
        {
            this.Client.Account.Delete(
                this.CompanyId,
                this.Client.Account.Retrieve(this.CompanyId).Single(a => a.Id.Equals(this.Id)),
                this.ToString());
        }

        public override string ToString()
        {
            return string.Format(
                @"Remove-Account -Id ""{0}""",
                this.Id);
        }
    }
}