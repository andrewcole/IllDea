namespace Illallangi.IllDea.PowerShell.Account
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Remove, Nouns.Account, SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    public sealed class RemoveAccountCmdlet : GetAccountCmdlet
    {
        protected override void ProcessRecord()
        {
            foreach (var account in this.Client.Account.Retrieve(this.CompanyId).Where(this.IsMatch))
            {
                this.Client.Account.Delete(
                    this.CompanyId,
                    account,
                    this.ToString());
            }
        }

        protected override bool IsMatch(IAccount account)
        {
            return base.IsMatch(account) &&
                   this.ShouldProcess(account.ToString(), VerbsCommon.Remove);
        }

        public override string ToString()
        {
            return string.Format(
                @"Remove-Account -Id ""{0}""",
                this.Id);
        }
    }
}