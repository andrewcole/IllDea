namespace Illallangi.IllDea.PowerShell.Account
{
    using System;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Set, Nouns.Account)]
    public sealed class SetAccountCmdlet : DeaCmdlet, IAccount
    {
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public Guid Id { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public AccountType Type { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true)]
        public string Number { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Account.Update(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"Set-Account -Id ""{0}"" -Name ""{1}"" -Type ""{2}"" -Number ""{3}""",
                this.Id,
                this.Name,
                this.Type,
                this.Number);
        }
    }
}