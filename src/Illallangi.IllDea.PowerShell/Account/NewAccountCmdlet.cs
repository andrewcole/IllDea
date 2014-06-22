namespace Illallangi.IllDea.PowerShell.Account
{
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    // http://www.netmba.com/accounting/fin/accounts/chart/

    [Cmdlet(VerbsCommon.New, Nouns.Account)]
    public sealed class NewAccountCmdlet : IdDeaCmdlet, IAccount
    {
        [Parameter(Mandatory = true, Position = 1)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public AccountType Type { get; set; }

        [Parameter(Mandatory = true)]
        public string Number { get; set; }

        public decimal Opening { get; set; }

        public decimal Closing { get; set; }

        protected override void ProcessRecord()
        {
            this.WriteObject(this.Client.Account.Create(this.CompanyId, this, this.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                @"New-Account -Name ""{0}"" -Type ""{1}"" -Number ""{2}""",
                this.Name,
                this.Type,
                this.Number);
        }
    }
}