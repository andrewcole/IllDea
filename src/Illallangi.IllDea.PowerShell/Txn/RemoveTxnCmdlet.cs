namespace Illallangi.IllDea.PowerShell.Txn
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    using Illallangi.IllDea.Model;

    [Cmdlet(VerbsCommon.Remove, Nouns.Txn, SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.High)]
    public sealed class RemoveTxnCmdlet : GetTxnCmdlet
    {
        protected override void ProcessRecord()
        {
            foreach (var txn in this.Client.Txn.Retrieve(this.CompanyId).Where(this.IsMatch))
            {
                this.Client.Txn.Delete(
                    this.CompanyId,
                    txn,
                    this.ToString());
            }
        }

        protected override bool IsMatch(ITxn txn)
        {
            return base.IsMatch(txn) &&
                   this.ShouldProcess(txn.ToString(), VerbsCommon.Remove);
        }

        public override string ToString()
        {
            return string.Format(
                @"Remove-Txn -Id ""{0}""",
                this.Id);
        }
    }
}